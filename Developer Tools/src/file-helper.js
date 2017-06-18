const fs = require('fs');

module.exports = {
    replace: (filepath, regex, replaceWith) => {
        return new Promise((resolve, reject) => {
            fs.readFile(filepath, 'utf8', function (err, data) {
                if (err) {
                    reject(err);
                    return;
                }

                const updatedFileContent = data.replace(regex, replaceWith);

                fs.writeFile(filepath, updatedFileContent, 'utf8', function (err) {
                    if (err) {
                        reject(err);
                        return;
                    }

                    resolve();
                });
            });
        });
    }
};