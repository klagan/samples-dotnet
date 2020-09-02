# Sample.HttpClient.Testing

Samples of an http retry handler and a sample of mocking responses from a http client which would be useful in testing when you want to stub the response.

## MyHttpMessageHandler

This class behaves like a stub.  It has a fixed response and will always return this response.  This is useful for testing, although I would opt for a crafted mock api server over this as a mock api server will ensure we test the transport and precludes any situation where test code is in the production code.a


## MYRetryMessageHandler

This class attempts to send the message three times if for any reason the message returns with a non successful response.
