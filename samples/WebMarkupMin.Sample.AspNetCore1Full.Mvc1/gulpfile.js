/*global require */
"use strict";

// include plug-ins
var gulp = require('gulp');
var del = require('del');
var sourcemaps = require('gulp-sourcemaps');
var rename = require('gulp-rename');
var concat = require('gulp-concat');
var less = require('gulp-less');
var autoprefixer = require('gulp-autoprefixer');
var cleanCss = require('gulp-clean-css');
var uglify = require('gulp-uglify');

var webRootPath = "wwwroot";
var bowerDirPath = webRootPath + "/lib";
var styleDirPath = webRootPath + '/styles';
var scriptDirPath = webRootPath + '/scripts';

//#region Clean
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

//#region Build assets
//#region Build styles
var autoprefixerOptions = {
	browsers: ['> 1%', 'last 3 versions', 'Firefox ESR', 'Opera 12.1'],
	cascade: true
};
var cssCleanOptions = { keepSpecialComments: '*' };
var cssRenameOptions = { extname: '.min.css' };

gulp.task('build-common-styles', function () {
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
		.pipe(cleanCss(cssCleanOptions))
		.pipe(rename(cssRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(styleDirPath + '/build'))
		;
});

gulp.task('build-styles', ['build-common-styles'], function () { });
//#endregion

//#region Build scripts
var jsConcatOptions = { newLine: ';' };
var jsUglifyOptions = {
	output: { comments: /^!/ }
};
var jsRenameOptions = { extname: '.min.js' };

gulp.task('build-modernizr-scripts', function () {
	return gulp
		.src([bowerDirPath + '/modernizr/modernizr.js'])
		.pipe(sourcemaps.init())
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(scriptDirPath + '/build'))
		;
});

gulp.task('build-common-scripts', function () {
	return gulp
		.src([bowerDirPath + '/bootstrap/js/dropdown.js',
			scriptDirPath + '/common.js'])
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('common-scripts.js', jsConcatOptions))
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest(scriptDirPath + '/build'))
		;
});

gulp.task('build-minification-form-scripts', function () {
	return gulp
		.src([bowerDirPath + '/jquery-validation/dist/jquery.validate.js',
			bowerDirPath + '/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
			bowerDirPath + '/bootstrap/js/button.js',
			scriptDirPath + '/minification-form.js'])
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
	return gulp.watch([styleDirPath + '/**/*.{less,css}', '!' + styleDirPath + '/build/**/*.*'],
		['build-styles']);
});

gulp.task('watch-scripts', function () {
	return gulp.watch([scriptDirPath + '/**/*.js', '!' + scriptDirPath + '/build/**/*.*'],
		['build-scripts']);
});

gulp.task('watch-assets', ['watch-styles', 'watch-scripts']);
//#endregion

//Set a default tasks
gulp.task('default', ['clean-builded-assets'], function () {
	return gulp.start('build-assets');
});