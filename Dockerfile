# syntax=docker/dockerfile:experimental
FROM nschultz/fantasy-baseball-common-models:0.5.4 AS build
COPY . /app
ENV MAIN_PROJ=FantasyBaseball.PlayerMergeService \
    SONAR_KEY=fantasy-baseball-player-merge
RUN --mount=type=cache,id=sonarqube,target=/root/.sonar/cache ./build.sh

FROM nschultz/base-csharp-runner:5.0.0
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "FantasyBaseball.PlayerMergeService.dll"]