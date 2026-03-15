#!/usr/bin/env bash
set -e

echo "Restaurando pacotes..."
dotnet restore IdentityCampaign.sln

echo "Restore concluído."