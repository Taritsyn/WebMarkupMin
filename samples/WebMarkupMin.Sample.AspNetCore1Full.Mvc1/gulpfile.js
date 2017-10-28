/// <binding BeforeBuild='build-assets' Clean='clean-builded-assets' ProjectOpened='copy-libraries, watch-assets' />
"use strict";

// include plug-ins
var gulp = require('gulp');
var del = require('del');
var copy = require('gulp-copy');
var sourcemaps = require('gulp-sourcemaps');
var rename = require('gulp-rename');
var concat = require('gulp-concat');
var less = require('gulp-less');
var autoprefixer = require('gulp-autoprefixer');
var minifyCss = require('gulp-minify-css');
var uglify = require('gulp-uglify');
var watch = require('gulp-watch');

var webRootPath = "./wwwroot";
var bowerDirPath = "../bower_components";
var initialBowerPrefix = 2;
var styleDirPath = webRootPath + '/styles';
var scriptDirPath = webRootPath + '/scripts';

//#region Clean
//#region Clean libraries
gulp.task('clean-style-libraries', function () {
	del.sync([styleDirPath + '/lib/*']);
});

gulp.task('clean-script-libraries', function () {
	del.sync([scriptDirPath + '/lib/*',
		'!' + scriptDirPath + '/lib/jquery.validate.unobtrusive.js']);
});

gulp.task('clean-libraries', ['clean-style-libraries', 'clean-script-libraries'], function () { });
//#endregion*

//#region Clean builded assets
gulp.task('clean-builded-styles', function () {
	del.sync([styleDirPath + '/build/*']);
});

gulp.task('clean-builded-scripts', function () {
	del.sync([scriptDirPath + '/build/*']);
});

gulp.task('clean-builded-assets', ['clean-builded-styles', 'clean-builded-scripts'], function () { });
//#endregion
//#endregion

//#region Copy libraries
//#region Copy style libraries
gulp.task('copy-bootstrap-styles', ['clean-style-libraries'], function () {
	return gulp
		.src([bowerDirPath + '/bootstrap/less/**/*.less'])
		.pipe(copy(styleDirPath + '/lib/bootstrap', { prefix: initialBowerPrefix + 2 }))
		;
});

gulp.task('copy-style-libraries', ['copy-bootstrap-styles'], function () { });
//#endregion

//#region Copy script libraries
gulp.task('copy-modernizr-scripts', ['clean-script-libraries'], function () {
	return gulp
		.src([bowerDirPath + '/modernizr/modernizr.js'])
		.pipe(copy(scriptDirPath + '/lib', { prefix: initialBowerPrefix }))
		;
});

gulp.task('copy-jquery-compat-scripts', ['clean-script-libraries'], function () {
	return gulp
		.src([bowerDirPath + '/jquery-compat/jquery.js', bowerDirPath + '/jquery-compat/jquery.min.js',
			bowerDirPath + '/jquery-compat/jquery.min.map'])
		.pipe(copy(scriptDirPath + '/lib/jquery-compat', { prefix: initialBowerPrefix }))
		;
});

gulp.task('copy-jquery-scripts', ['clean-script-libraries'], function () {
	return gulp
		.src([bowerDirPath + '/jquery/jquery.js', bowerDirPath + '/jquery/jquery.min.js',
			bowerDirPath + '/jquery/jquery.min.map'])
		.pipe(copy(scriptDirPath + '/lib/jquery', { prefix: initialBowerPrefix }))
		;
});

gulp.task('copy-jquery-validation-scripts', ['clean-script-libraries'], function () {
	return gulp
		.src([bowerDirPath + '/jquery-validation/dist/jquery.validate.js'])
		.pipe(copy(scriptDirPath + '/lib', { prefix: initialBowerPrefix + 1 }))
		;
});

gulp.task('copy-bootstrap-scripts', ['clean-script-libraries'], function () {
	return gulp
		.src([bowerDirPath + '/bootstrap/js/*.js'])
		.pipe(copy(scriptDirPath + '/lib/bootstrap', { prefix: initialBowerPrefix + 1 }))
		;
});

gulp.task('copy-script-libraries', ['copy-modernizr-scripts', 'copy-jquery-compat-scripts',
	'copy-jquery-scripts', 'copy-jquery-validation-scripts', 'copy-bootstrap-scripts'],
	function () { }
);
//#endregion

gulp.task('copy-libraries', ['copy-style-libraries', 'copy-script-libraries'], function () { });
//#endregion

//#region Build assets
//#region Build styles
var autoprefixerOptions = {
	browsers: ['> 1%', 'last 3 versions', 'Firefox ESR', 'Opera 12.1'],
	cascade: true
};
var cssMinifyOptions = { keepSpecialComments: '*' };
var cssRenameOptions = { extname: '.min.css' };

gulp.task('build-common-styles', ['clean-builded-styles'], function () {
	return gulp
		.src([styleDirPath + '/app.less'])
		.pipe(sourcemaps.init())
		.pipe(less({
			relativeUrls: true,
			rootpath: '/styles/'
		}))
		.pipe(autoprefixer(autoprefixerOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(styleDirPath + '/build'))
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('common-styles.css'))
		.pipe(minifyCss(cssMinifyOptions))
		.pipe(rename(cssRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(styleDirPath + '/build'))
		;
});

gulp.task('build-styles', ['build-common-styles'], function () { });
//#endregion

//#region Build scripts
var jsConcatOptions = { newLine: ';' };
var jsUglifyOptions = { preserveComments: 'some' };
var jsRenameOptions = { extname: '.min.js' };

gulp.task('build-modernizr-scripts', ['clean-builded-scripts'], function () {
	return gulp
		.src([scriptDirPath + '/lib/modernizr.js'])
		.pipe(sourcemaps.init())
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(scriptDirPath + '/build'))
		;
});

gulp.task('build-common-scripts', ['clean-builded-scripts'], function () {
	return gulp
		.src([scriptDirPath + '/lib/bootstrap/dropdown.js', scriptDirPath + '/common.js'])
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('common-scripts.js', jsConcatOptions))
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(scriptDirPath + '/build'))
		;
});

gulp.task('build-minification-form-scripts', ['clean-builded-scripts'], function () {
	return gulp
		.src([scriptDirPath + '/lib/jquery.validate.js', scriptDirPath + '/lib/jquery.validate.unobtrusive.js',
			scriptDirPath + '/lib/bootstrap/button.js', scriptDirPath + '/minification-form.js'])
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('minification-form-scripts.js', jsConcatOptions))
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(scriptDirPath + '/build'))
		;
});

gulp.task('build-scripts', ['build-modernizr-scripts', 'build-common-scripts',
	'build-minification-form-scripts'], function () { });
//#endregion

gulp.task('build-assets', ['build-styles', 'build-scripts'], function () { });
//#endregion

//#region Watch assets
gulp.task('watch-styles', function () {
	return gulp.watch([styleDirPath + '/**/*.{less,css}', '!' + styleDirPath + '/{lib,build}/**/*.*'],
		['build-styles']);
});

gulp.task('watch-scripts', function () {
	return gulp.watch([scriptDirPath + '/**/*.js', '!' + scriptDirPath + '/{lib,build}/**/*.*'],
		['build-scripts']);
});

gulp.task('watch-assets', ['watch-styles', 'watch-scripts']);
//#endregion

//Set a default tasks
gulp.task('default', ['copy-libraries'], function () {
	return gulp.start('build-assets');
});