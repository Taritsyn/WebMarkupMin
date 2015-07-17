/// <vs BeforeBuild='build-assets' Clean='clean-builded-assets' SolutionOpened='copy-libraries, watch-assets' />
module.exports = function (grunt) {
	// Project configuration
	grunt.initConfig({
		// Variables
		pkg: grunt.file.readJSON('package.json'),

		// Tasks
		clean: {
			'style-libraries': ['<%= pkg.styleDirPath %>/lib/*'],
			'script-libraries': [
				'<%= pkg.scriptDirPath %>/lib/*',
				'!<%= pkg.scriptDirPath %>/lib/jquery.validate.unobtrusive.js'
			],
			'builded-styles': ['<%= pkg.styleDirPath %>/build/*'],
			'builded-scripts': ['<%= pkg.scriptDirPath %>/build/*']
		},

		bowercopy: {
			options: {
				runBower: false,
				clean: false
			},
			styles: {
				options: {
					destPrefix: '<%= pkg.styleDirPath %>/lib'
				},
				files: [
					{ src: 'bootstrap/less/*.less', dest: 'bootstrap' },
					{ src: 'bootstrap/less/mixins/*.less', dest: 'bootstrap/mixins' }
				]
			},
			scripts: {
				options: {
					destPrefix: '<%= pkg.scriptDirPath %>/lib'
				},
				files: [
					{ src: 'modernizr/modernizr.js', dest: 'modernizr.js' },
					{ src: 'jquery-compat/jquery.js', dest: 'jquery-compat/jquery.js' },
					{ src: 'jquery-compat/jquery.min.js', dest: 'jquery-compat/jquery.min.js' },
					{ src: 'jquery-compat/jquery.min.map', dest: 'jquery-compat/jquery.min.map' },
					{ src: 'jquery/jquery.js', dest: 'jquery/jquery.js' },
					{ src: 'jquery/jquery.min.js', dest: 'jquery/jquery.min.js' },
					{ src: 'jquery/jquery.min.map', dest: 'jquery/jquery.min.map' },
					{ src: 'jquery-validation/dist/jquery.validate.js', dest: "jquery.validate.js" },
					{ src: 'bootstrap/js/*.js', dest: 'bootstrap' }
				]
			}
		},

		less: {
			options: {
				relativeUrls: true,
				rootpath: '/<%= pkg.styleDirPath %>/',
				sourceMap: true,
				sourceMapURL: 'app.css.map'
			},
			app: {
				options: {
					sourceMapFilename: '<%= pkg.styleDirPath %>/build/app.css.map'
				},
				files: [
					{
						src: '<%= pkg.styleDirPath %>/app.less',
						dest: '<%= pkg.styleDirPath %>/build/app.css'
					}
				]
			}
		},

		jshint: {
			options: {
				browser: true,
				ignores: [
					'<%= pkg.scriptDirPath %>/{lib,build}/**/*.*',
					'<%= pkg.scriptDirPath %>/_references.js'
				]
			},
			files: ['<%= pkg.scriptDirPath %>/**/*.js']
		},

		concat: {
			options: {
				sourceMap: true,
				sourceMapStyle: 'link'
			},
			styles: {
				files: [
					{
						src: ['<%= pkg.styleDirPath %>/build/app.css'],
						dest: '<%= pkg.styleDirPath %>/build/common-styles.css'
					}
				]
			},
			scripts: {
				options: {
					separator: ';'
				},
				files: [
					{
						src: [
							'<%= pkg.scriptDirPath %>/lib/bootstrap/dropdown.js',
							'<%= pkg.scriptDirPath %>/common.js'
						],
						dest: '<%= pkg.scriptDirPath %>/build/common-scripts.js'
					},
					{
						src: [
							'<%= pkg.scriptDirPath %>/lib/jquery.validate.js',
							'<%= pkg.scriptDirPath %>/lib/jquery.validate.unobtrusive.js',
							'<%= pkg.scriptDirPath %>/lib/bootstrap/button.js',
							'<%= pkg.scriptDirPath %>/minification-form.js'
						],
						dest: '<%= pkg.scriptDirPath %>/build/minification-form-scripts.js'
					}
				]
			}
		},

		cssmin: {
			options: {
				keepSpecialComments: '*',
				sourceMap: true
			},
			'common-styles': {
				files: [
					{
						src: '<%= pkg.styleDirPath %>/build/common-styles.css',
						dest: '<%= pkg.styleDirPath %>/build/common-styles.min.css'
					}
				]
			}
		},

		uglify: {
			options: {
				preserveComments: 'some',
				sourceMap: true
			},
			modernizr: {
				files: [
					{
						src: '<%= pkg.scriptDirPath %>/lib/modernizr.js',
						dest: '<%= pkg.scriptDirPath %>/build/modernizr.min.js'
					}
				]
			},
			'common-scripts': {
				options: {
					sourceMapIn: '<%= pkg.scriptDirPath %>/build/common-scripts.js.map'
				},
				files: [
					{
						src: '<%= pkg.scriptDirPath %>/build/common-scripts.js',
						dest: '<%= pkg.scriptDirPath %>/build/common-scripts.min.js'
					}
				]
			},
			'minification-form-scripts': {
				options: {
					sourceMapIn: '<%= pkg.scriptDirPath %>/build/minification-form-scripts.js.map'
				},
				files: [
					{
						src: '<%= pkg.scriptDirPath %>/build/minification-form-scripts.js',
						dest: '<%= pkg.scriptDirPath %>/build/minification-form-scripts.min.js'
					}
				]
			}
		},

		watch: {
			styles: {
				files: ['<%= pkg.styleDirPath %>/**/*.{less,css}', '!<%= pkg.styleDirPath %>/{lib,build}/**/*.*'],
				tasks: ['build-styles']
			},
			scripts: {
				files: ['<%= pkg.scriptDirPath %>/**/*.js', '!<%= pkg.scriptDirPath %>/{lib,build}/**/*.*'],
				tasks: ['build-scripts']
			}
		}
	});

	// Loading plugins
	grunt.loadNpmTasks('grunt-contrib-clean');
	grunt.loadNpmTasks('grunt-bowercopy');
	grunt.loadNpmTasks('grunt-contrib-less');
	grunt.loadNpmTasks('grunt-contrib-jshint');
	grunt.loadNpmTasks('grunt-contrib-concat');
	grunt.loadNpmTasks('grunt-contrib-cssmin');
	grunt.loadNpmTasks('grunt-contrib-uglify');
	grunt.loadNpmTasks('grunt-contrib-watch');

	// Registered tasks
	grunt.registerTask('clean-libraries', ['clean:style-libraries', 'clean:script-libraries']);
	grunt.registerTask('clean-builded-assets', ['clean:builded-styles', 'clean:builded-scripts']);

	grunt.registerTask('copy-style-libraries', ['clean:style-libraries', 'bowercopy:styles']);
	grunt.registerTask('copy-script-libraries', ['clean:script-libraries', 'bowercopy:scripts']);
	grunt.registerTask('copy-libraries', ['copy-style-libraries', 'copy-script-libraries']);

	grunt.registerTask('build-styles', ['clean:builded-styles', 'less', 'concat:styles', 'cssmin']);
	grunt.registerTask('build-scripts', ['clean:builded-scripts', 'jshint', 'concat:scripts', 'uglify']);
	grunt.registerTask('build-assets', ['build-styles', 'build-scripts']);

	grunt.registerTask('watch-assets', ['watch']);

	grunt.registerTask('default', ['copy-libraries', 'build-assets']);
}