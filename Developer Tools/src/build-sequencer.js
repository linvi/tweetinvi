const shell = require('./shell-helper');

const createSequencer = () => {
    const _actions = [];
    const _execute = (buildActions) => {
        const cwd = process.cwd();

        if (buildActions.length === 0) {
            return Promise.resolve();
        }

        const currentAction = buildActions.shift();
        let promise = currentAction().then(() => {
            return shell.cd(cwd, true); // Navigate back to source so that each action start with the same folder context
        });

        if (promise == null) {
            promise = Promise.resolve();
        }

        return promise.then(() => {
            return _execute(buildActions);
        });
    };

    return {
        execute: () => {
            return _execute(_actions);
        },
        addAction(promiseAction) {
            _actions.push(promiseAction);
        }
    };
};

module.exports = {
    create() {
        return createSequencer();
    }
}