const shell = require('./src/shell-helper');
const sequencerFactory = require('./src/build-sequencer');
const buildActions = require('./src/build-actions');

// shell.spawn('mkdir -p ./TweetinviAPI/lib/netstandard1.3');

const sequencer = sequencerFactory.create();

// sequencer.addAction(buildActions.updateProjectVersion);
// sequencer.addAction(buildActions.nugetRestore);
// sequencer.addAction(buildActions.buildDotNet);
sequencer.addAction(buildActions.createNugetPackage);

sequencer.execute();