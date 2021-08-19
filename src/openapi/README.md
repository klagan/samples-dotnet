
# My Sample Web API with ReDoc

This is a sample WebAPI documented using the docker verison of the open source [ReDoc](https://github.com/Redocly/redoc).

It works in accordance with the [OpenAPI](https://swagger.io/resources/open-api/) specification.

## Getting started

### Build containers

```zsh
docker-compose build
```

### Start up containers

```zsh
docker-compose up -d
```

### Access WebAPI deployment

Navigate to `http://localhost:5000`

### Access dynamic OpenAPI documentation

Navigate to `http://localhost`

### Access static OpenAPI documentation

Navigate to `http://localhost:5000/docs/v1/openapi.htm`

### Stop containers

```zsh
docker-compose down
```

### Create static (bundled) version of `redoc` documentation

```bash
# openapi.htm is the output name
# My OpenAPI is the tab name
# swagger.yaml is the original openAPI specification file
npx redoc-cli bundle --output openapi.htm --title "My OpenAPI" --ext yaml --options.theme.colors.primary.main=blue swagger.yaml
```

## JMeter tests

### Create a dashboard

- add `Simple Data Writer` to test plan/thread group
  
- add the following to /jmeter/bin/user.properties

```bash
jmeter.save.saveservice.bytes=true
jmeter.save.saveservice.label=true
jmeter.save.saveservice.latency=true
jmeter.save.saveservice.response_code=true
jmeter.save.saveservice.response_message=true
jmeter.save.saveservice.successful=true
jmeter.save.saveservice.thread_counts=true
jmeter.save.saveservice.thread_name=true
jmeter.save.saveservice.time=true
jmeter.save.saveservice.timestamp_format=ms
jmeter.save.saveservice.timestamp_format=yyyy/MM/dd HH:mm:ss
jmeter.save.saveservice.print_field_names=true
```

- run the following shell command

```bash
# -n non gui mode
# -t test file
# -l logfile
# -e generate report dashboard after the load test
# -f delete log file before running tests
# -o dashboard folder output
jmeter -n -t sample.jmx -l results.jtl -e -f -o dashboard
```

## Acknowledgements

- [ReDoc](https://github.com/Redocly/redoc)
- [Swagger](https://swagger.io/resources/open-api/)

## Authors

- [@klagan](https://github.com/klagan)

## Badges

[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/tterb/atomic-design-ui/blob/master/LICENSEs)

## Contributing

Contributions are always welcome!
