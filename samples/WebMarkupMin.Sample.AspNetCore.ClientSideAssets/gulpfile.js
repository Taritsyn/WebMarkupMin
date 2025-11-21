/*global require, exports */
/*jshint esversion: 6 */
const WEB_ROOT_PATH = "wwwroot";
const BOWER_DIR_PATH = WEB_ROOT_PATH + "/lib";
const STYLE_DIR_PATH = WEB_ROOT_PATH + '/styles';
const SCRIPT_DIR_PATH = WEB_ROOT_PATH + '/scripts';

// include plug-ins
let { src, dest, series, parallel, watch } = require('gulp');
let del = require('del');
let sourcemaps = require('gulp-sourcemaps');
let rename = require('gulp-rename');
let concat = require('gulp-concat');
let less = require('gulp-less');
let autoprefixer = require('gulp-autoprefixer');
let cleanCss = require('gulp-clean-css');
let uglify = require('gulp-uglify');

//#region Clean
//#region Clean builded assets
function cleanBuildedStyles() {
	return del([STYLE_DIR_PATH + '/build/*']);
}

function cleanBuildedScripts() {
	return del([SCRIPT_DIR_PATH + '/build/*']);
}

let cleanBuildedAssets = parallel(cleanBuildedStyles, cleanBuildedScripts);
//#endregion
//#endregion

//#region Build assets
//#region Build styles
let autoprefixerOptions = {
	overrideBrowserslist: ['> 1%', 'last 3 versions', 'Firefox ESR', 'Opera 12.1'],
	cascade: true
};
let cssCleanOptions = { specialComments: '*' };
let cssRenameOptions = { extname: '.min.css' };

function buildCommonStyles() {
	return src([STYLE_DIR_PATH + '/app.less'])
		.pipe(sourcemaps.init())
		.pipe(less({
			relativeUrls: true,
			rootpath: '/styles/'
		}))
		.pipe(autoprefixer(autoprefixerOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(dest(STYLE_DIR_PATH + '/build'))
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('common-styles.css'))
		.pipe(cleanCss(cssCleanOptions))
		.pipe(rename(cssRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(dest(STYLE_DIR_PATH + '/build'))
		;
}

let buildStyles = buildCommonStyles;
//#endregion

//#region Build scripts
let jsConcatOptions = { newLine: ';' };
let jsUglifyOptions = {
	output: { comments: /^!/ }
};
let jsRenameOptions = { extname: '.min.js' };

function buildModernizrScripts() {
	return src([BOWER_DIR_PATH + '/modernizr/modernizr.js'])
		.pipe(sourcemaps.init())
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(dest(SCRIPT_DIR_PATH + '/build'))
		;
}

function buildCommonScripts() {
	return src([BOWER_DIR_PATH + '/bootstrap/js/dropdown.js',
			SCRIPT_DIR_PATH + '/common.js'])
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('common-scripts.js', jsConcatOptions))
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(dest(SCRIPT_DIR_PATH + '/build'))
		;
}

function buildMinificationFormScripts() {
	return src([BOWER_DIR_PATH + '/jquery-validation/dist/jquery.validate.js',
			BOWER_DIR_PATH + '/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
			BOWER_DIR_PATH + '/bootstrap/js/button.js',
			SCRIPT_DIR_PATH + '/minification-form.js'])
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(concat('minification-form-scripts.js', jsConcatOptions))
		.pipe(uglify(jsUglifyOptions))
		.pipe(rename(jsRenameOptions))
		.pipe(sourcemaps.write('./'))
		.pipe(dest(SCRIPT_DIR_PATH + '/build'))
		;
}

let buildScripts = parallel(buildModernizrScripts, buildCommonScripts,
	buildMinificationFormScripts);
//#endregion

let buildAssets = parallel(buildStyles, buildScripts);
//#endregion

//#region Watch assets
function watchStyles() {
	return watch([STYLE_DIR_PATH + '/**/*.{less,css}', '!' + STYLE_DIR_PATH + '/build/**/*.*'],
		buildStyles);
}

function watchScripts() {
	return watch([SCRIPT_DIR_PATH + '/**/*.js', '!' + SCRIPT_DIR_PATH + '/build/**/*.*'],
		buildScripts);
}

let watchAssets = parallel(watchStyles, watchScripts);
//#endregion

// Export tasks
exports.cleanBuildedAssets = cleanBuildedAssets;
exports.buildAssets = buildAssets;
exports.watchAssets = watchAssets;
exports.default = series(cleanBuildedAssets, buildAssets);