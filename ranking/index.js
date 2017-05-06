'use strict';
var database = require('./database');
var express = require('express');
var bodyParser = require('body-parser');
var Score = require('./database').Score;


database.connect(function(err) {


	var app = express();
	app.use(bodyParser.json());

	app.get("/api/v1/ranking", function(req,res){
		 let q= Score.find({}).sort({'score': -1}).limit(5);
				q.exec(function(err, doc) {
		        		console.log(doc);
           			 	res.send(doc);
           			 })

	});


  app.get("/api/v1/ranking/:id", function(req,res){
		let  id = req.params.id;
		 let q= Score.findOne({id:id}).sort({'score': -1}).limit(5);
				q.exec(function(err, doc) {
		        		console.log(doc);
           			 	res.send(doc);
           			 })

	});


	app.post("/api/v1/ranking",function(req,res){
		let object = req.body;
		console.log(object);
		let sc = new Score(object);
		sc.save().then(function(){
				res.sendStatus(201);

		})

	});

	var port = (process.env.PORT || 3000); 
	app.listen(port);

});