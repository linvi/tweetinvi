const glob = require('glob');
const path = require('path');

const shell = require('./shell-helper');
const fileHelper = require('./file-helper');
const buildInfos = require('./build-properties');

module.exports = {
    updateProjectVersion: () => {
        const projectFilePaths = glob.sync("../**/*.csproj");
        const assemblyInfosFilePaths = glob.sync('../**/AssemblyInfo.cs');
        const nuget = path.resolve('./TweetinviAPI/TweetinviAPI.nuspec');
        const clientHandler = path.resolve('../Tweetinvi.WebLogic/TwitterClientHandler.cs');

        var allUpdates = [];

        allUpdates = projectFilePaths.map(filepath => {
            return fileHelper.replace(filepath, /<VersionPrefix>([0-9\.]+[0-9])<\/VersionPrefix>/g, `<VersionPrefix>${buildInfos.version}</VersionPrefix>`).then(() => {
                console.log(filepath + ' version has been updated!');
            });
        }).concat(allUpdates);

        allUpdates = assemblyInfosFilePaths.map(filepath => {
            return fileHelper.replace(filepath, /\[assembly: (AssemblyVersion|AssemblyFileVersion)\("([0-9\.]+[0-9])"\)\]/g, `[assembly: $1("${buildInfos.version}")]`).then(() => {
                console.log(filepath + ' version has been updated!');
            });
        }).concat(allUpdates);

        allUpdates.push(fileHelper.replace(nuget, /<version>[0-9\.]+[0-9]<\/version>/g, `<version>${buildInfos.version}</version>`).then(() => {
            return fileHelper.replace(
                nuget,
                /<releaseNotes>https:\/\/github.com\/linvi\/tweetinvi\/releases\/tag\/[0-9\.]*<\/releaseNotes>/g,
                `<releaseNotes>https://github.com/linvi/tweetinvi/releases/tag/${buildInfos.version}</releaseNotes>`)
        }).then(() => {
            console.log('Nuget spec has been updated.')
        }));

        allUpdates.push(fileHelper.replace(clientHandler, /"Tweetinvi\/(\d+(\.\d+)*)(.x)?"/, `"Tweetinvi/${buildInfos.version}"`).then(() => {
            console.log('User agent updated!');
        }));

        return Promise.all(allUpdates).then(() => {
            console.log('');
            console.log('******************************************');
            console.log('');
        });
    },

    nugetRestore: () => {
        return shell.cd('../tweetinvi').then(() => {
        }).then(() => {
            return shell.spawn('dotnet restore');
        });
    },

    buildDotNet: () => {
        return shell.cd('../tweetinvi').then(() => {
        }).then(() => {
            return shell.spawn('dotnet build');
        });
    },

    createNugetPackage: () => {
        return shell.spawn('mkdir TweetinviAPI\\lib\\netstandard1.3').then(() => {
            return shell.spawn('cp ../Tweetinvi/bin/Debug/netstandard1.3/Tweetinvi*.dll ./TweetinviAPI/lib/netstandard1.3');
        }).then(() => {
            return shell.spawn('cp ../Tweetinvi/bin/Debug/netstandard1.3/Tweetinvi.*.dll ./TweetinviAPI/lib/net46');
        }).then(() => {
            return shell.cd('./TweetinviAPI');
        }).then(() => {
            return shell.spawn('rimraf *.nupkg');
        }).then(() => {
            return shell.spawn('ls');
        }).then(() => {
            return shell.cd('../');
        }).then(() => {
            return shell.spawn('nuget.exe pack ./TweetinviAPI/TweetinviAPI.nuspec -OutputDirectory TweetinviAPI');
        });
    }
};