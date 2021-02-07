# syntax=docker/dockerfile:experimental
FROM nschultz/fantasy-baseball-common:0.6.0 AS build
COPY . /app
ENV MAIN_PROJ=FantasyBaseball.PlayerMergeService \
    SONAR_KEY=fantasy-baseball-player-merge
RUN --mount=type=cache,id=sonarqube,target=/root/.sonar/cache ./build.sh

FROM nschultz/base-csharp-runner:5.0.0
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "FantasyBaseball.PlayerMergeService.dll"]