## Integration tests with postman

This small "API" should demonstrate the power of postman (in combination with newman). Postman itself is a request runner based on google chrome. You can send any request you desire to any api and even modify them with a proxy in between. If you want to automate your integration tests, you can export the created collection and run it with newman in your CI-environment.

## Setup

```bash
npm install -g newman
npm install
npm start
```

Your demo api is running now.

## Execute tests

Since the api is only for demo cases, after each run (with or without data file) you need to restart the api so the objects array is emptied :wink:.

### Without a data file

Executing without a datafile means you run your testcollection once.

```bash
newman -c POSTMAN.postman_collection.json --global-var "BASE_URI=http://127.0.0.1:5000" 
```

**Benchmark Test Example**

Test benchmark http://127.0.0.1:5000 with concurrent thread (connection) 10 for 60 seconds

    siege -c10 -t60s http://127.0.0.1:5000/api/customers

More Information visit author of siege http://www.joedog.org/siege-home/

[Download zip](https://github.com/ewwink/siege-windows/blob/master/siege-windows-3.0.5.zip)
