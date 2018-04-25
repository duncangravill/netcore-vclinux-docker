# netcore-vclinux-docker

A Visual Studio solution combining a .net core console project and a vclinux project. The aim is to configure the solution and projects so that they build and are debuggable in a shared docker container. The VC Linux project builds a simple shared object library which exports a function `extern "C" int Multiply(int x, int y)`. The .net Core Console project P/Invokes into the shared object library. Currently both the .net Core Console project and the VC Linux project build in the shared docker container and the .net Core Console app is debuggable. The P/Invoke call is successful. **The next step is to configure debugging for stepping into the VC Linux project.**

### Steps to build and run the solution

1. Use Visual Studio 2017 15.7 Preview 4 or 15.6, with cross platform .net core development capabilities as well as cross platform linux development capabilities.

2. Once the solution has loaded in VS and the docker contain is running, find out the id of the docker container, run the command `docker exec -d CONTAINER_ID /usr/sbin/sshd -D`. Now sshd will be listening so that the VC Linux project will be able to build.

3. If upon building VC Linux asks for connection credentials use username:root, hostname:127.0.0.1, port:12345, password:toor.

4. The project was created in VS 15.7 Preview 4. If you are using VS 15.6 you might need to change the Remote Post Build Event in the VC Linux project properties to `cp bin/x64/Debug/libSIMPLEVCLINUX.so.1.0 /app/libSIMPLEVCLINUX.so.1.0`


### Steps taken so far in setting up the projects

The following lines have been added to the Dockerfile:

```
RUN apt-get update && apt-get install -y build-essential openssh-server gdb gdbserver && mkdir /var/run/sshd
RUN echo 'root:toor' | chpasswd


RUN sed -i 's/#PermitRootLogin prohibit-password/PermitRootLogin yes/' /etc/ssh/sshd_config && \
 	sed -i 's@session\s*required\s*pam_loginuid.so@session optional pam_loginuid.so@g' /etc/pam.d/sshd && \
 	apt-get clean

EXPOSE 22
```


The following lines have been added to docker-compose:

```
    ports:
      - "12345:22"
    security_opt:
      - seccomp:unconfined
```

These Dockerfile and docker-compose changes enable VC Linux to build and potentially to debug. This solution is rough and really only potentially good for debugging, a different Dockerfile should be used for producing a release docker image. 




