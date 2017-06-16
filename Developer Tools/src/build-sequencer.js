const shell = require('./shell-helper');

const createSequencer = () => {
    const _actions = [];
    const _execute = (buildActions) => {
        if (buildActions.length === 0) {
            return Promise.resolve();
        }

        const currentAction = buildActions.shift();
        let promise = currentAction();

        if (promise == null) {
            promise = Promise.resolve();
        }

        return promise.then(() => {
            return _execute(buildActions)
        });
    };

    return {
        execute: () => {
            const cwd = process.cwd();

            return _execute(_actions).then(() => {
                return shell.cd(cwd);
            });
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