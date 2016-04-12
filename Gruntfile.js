module.exports = function(grunt) {

	var path = require('path');

	var pkg = grunt.file.readJSON('package.json');

	var projectRoot = 'src/' + pkg.name + '/';

	var assembly = grunt.file.readJSON(projectRoot + 'Properties/AssemblyInfo.json');

	var version = assembly.informationalVersion ? assembly.informationalVersion : assembly.version;

	grunt.initConfig({
		pkg: pkg,
		zip: {
			release: {
				router: function (filepath) {
					filepath = filepath.replace(projectRoot, '');
					var filename = path.basename(filepath);
					return filepath.match(/^bin\//) ? 'bin/' + filename : filepath;
				},
				src: [
					projectRoot + 'bin/Release/Skybrud.Social.dll',
					projectRoot + 'bin/Release/Skybrud.Social.xml',
					projectRoot + 'bin/Release/Skybrud.WebApi.Json.dll',
					projectRoot + 'bin/Release/Skybrud.WebApi.Json.xml',
					projectRoot + 'bin/Release/' + pkg.name + '.dll',
					projectRoot + 'bin/Release/' + pkg.name + '.xml',
					projectRoot + 'App_Plugins/**/*.*'
				],
				dest: 'releases/' + pkg.name + '.v' + version + '.zip'
			}
		},
		nugetpack: {
			dist: {
				src: 'src/' + pkg.name + '/' + pkg.name + '.csproj',
				dest: 'nuget/'
			}
		}
	});

	grunt.loadNpmTasks('grunt-contrib-copy');
	grunt.loadNpmTasks('grunt-nuget');
	grunt.loadNpmTasks('grunt-zip');

	grunt.registerTask('dev', ['zip', 'nugetpack']);
	grunt.registerTask('release', ['zip', 'nugetpack']);

	grunt.registerTask('default', ['dev']);

};