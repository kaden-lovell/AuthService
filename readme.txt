Follow steps, order can be important

Pre-Req:
Enable virtualization in the BIOS
Enable WSL 2 (Ubuntu, any version)
Start WSL 2

Install:
Dotnet Core 7.0 (check version
Docker Desktop
Visual Studio (latest year version)
Resharper (license provided)

Commands to know (ignore `` when copying):
`docker build -t <project directory name> .`
`docker images`
`docker run -it --rm -p <port:protocol> --name <mymicroservicecontainer> mymicroservice`


Build Commands/Procedure:
docker build -t AuthService
docker run -it --rm -p <3000:80> --name authservicecontainer authservice