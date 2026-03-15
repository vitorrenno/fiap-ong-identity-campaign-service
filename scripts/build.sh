#!/usr/bin/env bash
set -e

echo "Compilando solução..."
dotnet build IdentityCampaign.sln --configuration Release

echo "Build concluído."