# Запуск приложения и observability-стека

Команды ниже рассчитаны на локальный `kind`. Нужны `docker`, `kind`, `kubectl`, `helm` и .NET SDK 10.

## 1. Создать кластер

```bash
kind create cluster --name observability-course
kubectl config use-context kind-observability-course
```

## 2. Установить Prometheus, kube-state-metrics, Grafana, Loki, Tempo и Alloy

```bash
helm repo add grafana-community https://grafana-community.github.io/helm-charts
helm repo add grafana https://grafana.github.io/helm-charts
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
bash scripts/observability/install.sh install
```

`install.sh render` вместо `install` выводит манифесты, ничего не устанавливая.

## 3. Собрать и установить приложение

Из корня репозитория:

```bash
docker build -t feedback-quiz:dev _Src/GFeed.WebApiApp
kind load docker-image feedback-quiz:dev --name observability-course
helm upgrade --install feedback-quiz charts/feedback-quiz \
  --namespace homework-2 --create-namespace \
  --set-string image=feedback-quiz:dev --wait --timeout 5m
```

Chart задаёт `OTEL_SERVICE_NAME=feedback-quiz` и отправляет метрики и логи через OTLP/gRPC в `alloy.monitoring.svc.cluster.local:4317`. HTTP server request count, duration и status code метрики поступают из ASP.NET Core instrumentation. Это покрывает RED: rate, errors и duration; runtime instrumentation также добавляет метрики .NET.

## 4. Открыть Grafana и приложение

В отдельных терминалах:

```bash
kubectl -n monitoring port-forward svc/grafana 3000:80
kubectl -n homework-2 port-forward svc/feedback-quiz 8080:80
```

Grafana: http://127.0.0.1:3000 (логин `admin`, пароль `observability-course`). Приложение: http://127.0.0.1:8080. Создайте нагрузку, например:

```bash
for i in {1..20}; do curl -sS http://127.0.0.1:8080/ >/dev/null; done
```

При недоступном локальном OTLP endpoint разработчик может запускать приложение вне Kubernetes, установив `OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317` и пробросив порт Alloy командой `kubectl -n monitoring port-forward svc/alloy 4317:4317`.
