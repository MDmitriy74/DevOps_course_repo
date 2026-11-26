#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$ROOT/versions.sh"
MODE="${1:-render}"
if [[ "$MODE" != render && "$MODE" != install ]]; then
  echo "Usage: bash scripts/observability/install.sh [render|install]" >&2
  exit 1
fi
if [[ "$MODE" == install ]]; then
  echo "Installing into context: $(kubectl config current-context), namespace: monitoring"
fi
deploy() {
  local release="$1" chart="$2" version="$3"
  shift 3
  if [[ "$MODE" == render ]]; then
    helm template "$release" "$chart" --version "$version" \
      --namespace monitoring -f "$ROOT/values/$release.yaml" "$@"
  else
    helm upgrade --install "$release" "$chart" --version "$version" \
      --namespace monitoring --create-namespace \
      -f "$ROOT/values/$release.yaml" "$@" --wait --timeout 10m
  fi
}
deploy prometheus prometheus-community/prometheus "$PROMETHEUS_CHART_VERSION"
deploy loki grafana-community/loki "$LOKI_CHART_VERSION"
deploy tempo grafana-community/tempo "$TEMPO_CHART_VERSION"
deploy alloy grafana/alloy "$ALLOY_CHART_VERSION"
deploy grafana grafana-community/grafana "$GRAFANA_CHART_VERSION" \
  --set-file "dashboards.course.kubernetes.json=$ROOT/dashboards/kubernetes.json" \
  --set-file "dashboards.course.grafana.json=$ROOT/dashboards/grafana.json"
