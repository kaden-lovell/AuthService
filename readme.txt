Follow steps, order can be important

Pre-Req:
Enable virtualization in the BIOS
Enable WSL 2 (Ubuntu, any version)
Start WSL 2

Install:
Dotnet Core 7.0 (check version of project)
Docker Desktop (latest version)
Visual Studio (latest version)
Resharper (license provided)

Commands to know (ignore `` when copying):
`docker build -t <project directory name> .`
`docker images`
`docker run -it --rm -p <port:protocol> --name <mymicroservicecontainer> mymicroservice`


Build Commands/Procedure (Docker image should be maintained through all stages of project lifecycle):
-- Local Testing
dotnet build
dotnet run

-- Before Pushing
docker build -t AuthService
docker run -it --rm -p <3000:80> --name authservicecontainer authservice