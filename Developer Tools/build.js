const shell = require('./src/shell-helper');
const sequencerFactory = require('./src/build-sequencer');
const fileHelper = require('./src/file-helper');
const glob = require('glob');
const path = require('path');

const invokerCWD = process.cwd();

const buildInfos = {
    version: '2.0.0.0',
    buildProject: 'Tweetinvi'
};

const sequencer = sequencerFactory.create();
sequencer.addAction(updateProjectVersion);
sequencer.addAction(buildDotNet);
sequencer.execute();

function updateProjectVersion() {
    const projectFilePaths = glob.sync("../**/*.csproj");
    const assemblyInfosFilePaths = glob.sync('../**/AssemblyInfo.cs');
    const nuget = path.resolve('./TweetinviAPI/TweetinviAPI.nuspec');
    const clientHandler = path.resolve('../Tweetinvi.WebLogic/TwitterClientHandler.cs');

    projectFilePaths.forEach(filepath => {
        fileHelper.replace(filepath, /<VersionPrefix>([0-9\.]+[0-9])<\/VersionPrefix>/g, `<VersionPrefix>${buildInfos.version}</VersionPrefix>`).then(() => {
            console.log(filepath + ' version has been updated!');
        });
    });

    assemblyInfosFilePaths.forEach(filepath => {
        fileHelper.replace(filepath, /\[assembly: (AssemblyVersion|AssemblyFileVersion)\("([0-9\.]+[0-9])"\)\]/g, `[assembly: $1("${buildInfos.version}")]`).then(() => {
            console.log(filepath + ' version has been updated!');
        });
    });

    fileHelper.replace(nuget, /<version>[0-9\.]+[0-9]<\/version>/g, `<version>${buildInfos.version}</version>`).then(() => {
        return fileHelper.replace(
            nuget,
            /<releaseNotes>https:\/\/github.com\/linvi\/tweetinvi\/releases\/tag\/[0-9\.]*<\/releaseNotes>/g,
            `<releaseNotes>https://github.com/linvi/tweetinvi/releases/tag/${buildInfos.version}</releaseNotes>`)
    }).then(() => {
        console.log('Nuget spec has been updated.')
    });

    fileHelper.replace(clientHandler, /"Tweetinvi\/(\d+(\.\d+)*)(.x)?"/, `"Tweetinvi/${buildInfos.version}"`).then(() => {
        console.log('User agent updated!');
    });
};

function buildDotNet() {
    return shell.cd('../tweetinvi').then(() => {
    }).then(() => {
        return shell.spawn('dotnet build');
    });
}

// const filepaths = glob.sync("../**/*.csproj");
// filepaths.forEach(filepath => {
//     console.log(filepath);
// });