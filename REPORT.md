# ДЗ 1. Своё приложение в Kubernetes

## 1. Данные автора

- ФИО: **Мостовов Дмитрий Сергеевич**
- Группа: **ФТ-303**
- ОС: Windows 11 с WSL2
- Архитектура: `amd64`

## 2. Приложение

Скопировал приложение с проектного практикума (ASP.NET Core), удалил всё не нужное.
Оставил один контроллер, дополнил его методами для задания 

слушает `8080` внутри контейнера.
переменные окружения:

  - APP_TOKEN из Secret.
 - APP_MESSAGE: "Feedback quiz configuration was changed"
 -  APP_AUTHOR: "Dima"
 - ASPNETCORE_ENVIRONMENT: "Production"

код находится в `_Src/GFeed.WebApiApp`, Dockerfile - там же, Kubernetes - в `k8s`.

## 3. Сборка и публикация образа

команды выполнял из разных мест.... (wsl, каталоги, Power Shell) Надеюсь будут работать

PS D:\Dima\3University\DevOps\feedback-quiz-5th-floor\_Src\GFeed.WebApiApp> docker build -t localhost:5001/dima-feedback-quiz:v2 .

docker push localhost:5001/dima-feedback-quiz:v2

cd ..   cd ..





```bash
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/deployment.yaml
```

kind load docker-image localhost:5001/dima-feedback-quiz:v2 --name hosting-lab

kubectl -n homework-1 rollout status deployment/feedback-quiz


kubectl -n homework-1 get pods -o wide

curl -i http://localhost:8082/ 

## 4.

тег: localhost:5001/dima-feedback-quiz:v2

push: ...
32ab39660fd5: Layer already exists
v2: digest: sha256:55dfcb54f3bcf07b0fdc9f966d6736a07f6d2b431a2c2d13a4f6d80959423339 size: 2203

## 5.

```bash
kubectl -n homework-1 port-forward deployment/feedback-quiz 8082:8080
```

```bash
curl -sS http://localhost:8082/
```
kubectl -n homework-1 delete pod  feedback-quiz-58fdb5b98f-gj4j6

 curl -i http://localhost:8082/
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Fri, 23 Oct 2026 08:39:12 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"message":"Feedback quiz is running in Kubernetes","author":"Dima","tokenCod

после второго эксперимента:

 kubectl -n homework-1 apply -f k8s/configmap.yaml


url -sS http://localhost:8082/
{"message":"Feedback quiz configuration was changed","author":"Dima","tokenConfigured":true}d

#### Эксперимент 1:

До удаления Pod:

```
NAME                             UID                                    RESTARTS
feedback-quiz-58fdb5b98f-gj4j6   2d01eae0-3764-49cd-a491-e5c626aad37c   0
feedback-quiz-58fdb5b98f-tn8rj   79d78d45-05e5-49af-9871-815315a65219   0
```

После удаления одного Pod:

```
NAME                             UID                                    RESTARTS
feedback-quiz-58fdb5b98f-ns4c6   4f730dfe-1a88-42a8-b7cb-ea797c0cb39b   0
feedback-quiz-58fdb5b98f-tn8rj   79d78d45-05e5-49af-9871-815315a65219   0

```

Новый Pod создал ReplicaSet, которым управляет Deployment. Deployment поддерживает заданное число реплик, и
после удаления одного Pod создался новый с новым uid и именем. (А при перезапуске контейнера UID Pod сохранился бы, а увеличился бы
`RESTARTS`)



#### Эксперимент 2:

было

```text
{"message":"Feedback quiz is running in Kubernetes","author":"Dima","tokenConfigured":true}
```

После изменения `APP_MESSAGE` и применения ConfigMap, до перезапуска
Deployment, запущенное приложение продолжило возвращать старое значение

Команды изменения и перезапуска:

После перезапуска Deployment:

```text
{"message":"Feedback quiz configuration was changed","author":"Dima","tokenConfigured":true}
```

Изменение ConfigMap само не изменило переменные окружения уже
запущенного процесса. При создании новых Pod Deployment передал им актуальное
значение, поэтому после `rollout restart` появилось новое сообщение.

### Короткие ответы

-Labels маркируют объекты и позволяют выбирать их по
  признаку. selector в Deployment должен совпадать с labels у Pod, чтобы
  Deployment управлял именно своими Pod

- Requests - гарантируемый объём ресурсов, при размещении Pod. Limits - верхняя граница потребления контейнера
- СonfigMap предназначен для обычной конфигурации,
  Secret - для чувствительных значений
- Base64 - это кодирование, а не шифрование. (исходное значение можно раскодировать)

### Удаление ресурсов работы


```bash
kubectl delete -f k8s/deployment.yaml
kubectl delete -f k8s/secret.yaml
kubectl delete -f k8s/configmap.yaml
kubectl delete -f k8s/namespace.yaml
```