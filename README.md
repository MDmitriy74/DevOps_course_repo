Сервис обратной связи и аналитики для Молодёжного бюро «5 этаж»
Назначение проекта. Создать сервис для сбора обратной связи после мероприятий и отображения агрегированной аналитики для участников и организаторов.
1. Цель проекта
Разработать веб-сервис, который позволяет быстро собрать обратную связь участников мероприятия и сразу после отправки формы показать участнику обновлённую агрегированную статистику.
Для организаторов сервис должен предоставлять данные по отдельным мероприятиям, позволять анализировать результаты и экспортировать собранные данные.

## DevOps 4.1: Docker-образ приложения

Приложение предоставляет `GET /`. Оно возвращает значение `APP_MESSAGE`, автора
и только признак наличия `APP_TOKEN` (`tokenConfigured`); само значение токена
никогда не возвращается. Контейнер слушает `0.0.0.0:8080`.

Команды выполняются из каталога `_Src/GFeed.WebApiApp`:

```powershell
docker build -t localhost:5001/dima-feedback-quiz:v1 .
docker push localhost:5001/dima-feedback-quiz:v1
docker run --rm -p 8080:8080 `
  -e APP_MESSAGE="Feedback quiz is running" `
  -e APP_TOKEN=" учебный-секрет " `
  localhost:5001/dima-feedback-quiz:v1
```

Проверка ответа:

```powershell
curl http://localhost:8080/
```

Ожидаемый результат содержит `message`, `author: "Dima"` и
`tokenConfigured: true`, но не содержит значение `APP_TOKEN`.

Перед push должен быть запущен локальный Registry, например:

```powershell
docker run -d --restart=always -p 5001:5000 --name local-registry registry:2
```

При изменении приложения используйте новый тег (`v2`, `v3`, ...) и повторите
команды сборки и push.

## DevOps 4.2: Kubernetes-манифесты

Манифесты находятся в каталоге `k8s`. Deployment запускает две реплики
образа `localhost:5001/dima-feedback-quiz:v2`, передаёт `APP_MESSAGE` из
ConfigMap и `APP_TOKEN` из Secret и задаёт requests/limits для CPU, памяти и
ephemeral-storage.

Применение и проверка:

```powershell
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/deployment.yaml
kubectl -n homework-1 rollout status deployment/feedback-quiz
kubectl -n homework-1 get pods -o wide
```

Проверка HTTP выполняется в двух терминалах. В первом:

```powershell
kubectl -n homework-1 port-forward deployment/feedback-quiz 8082:8080
```

Во втором:

```powershell
curl.exe -i http://localhost:8082/
```
