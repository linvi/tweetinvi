const nodeExec = require('child_process').exec;
const nodeSpawn = require('child_process').spawn;

module.exports = {
    exec: (cmd) => {

        return new Promise((resolve, reject) => {
            console.log(`> ${cmd}`);

            const execution = nodeExec(cmd, {}, (error, stdout, stderr) => {
                if (error) { console.error(`exec error: ${error}`); reject(error); return; }
                // if (stdout) { console.log(stdout); }
                if (stderr) { console.log(stderr); }
                resolve();
            });

            execution.stdout.on('data', function (data) {
                process.stdout.write(data);
            });
        });
    },
    spawn: (cmd) => {
        return new Promise((resolve, reject) => {
            console.log(`> ${cmd}`);

            const execution = nodeSpawn('cmd.exe', ['/s', '/c', cmd], {
                stdio: 'inherit'
            });

            execution.on('exit', code => { resolve(code); });
        });
    },
    cd: (path, silent) => {
        return new Promise((resolve, reject) => {
            try {
                process.chdir(path);

                if (!silent) {
                    console.log(`cd ${process.cwd()}`);
                }

                resolve();
            }
            catch (error) {
                reject(error);
            }
        });
    }
};