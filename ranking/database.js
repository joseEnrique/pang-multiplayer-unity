'use strict';

var mongoose = require('mongoose');
var fs = require('fs');
var jsyaml = require('js-yaml');
var Schema = mongoose.Schema;
var Score = fs.readFileSync(__dirname + '/score.yaml', 'utf8');
console.log(jsyaml.safeLoad(Score))

module.exports = {
    connect: _connect,
    Score: mongoose.model('Score', new Schema(jsyaml.safeLoad(Score)))
   
};

function _connect(callback) {
    let host = "178.32.158.123";
    let port = "27017";
    let name = "ranking"

    var url = 'mongodb://' + host + ':' + port + '/' + name;
    console.log(url)
    mongoose.connect(url, function (err) {
        if (!err) {
            callback();
        } else {
            callback(err);
        }
    });
}