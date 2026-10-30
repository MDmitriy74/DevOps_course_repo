# ДЗ 1. Своё приложение в Kubernetes

## 1. Данные автора

- ФИО: **Мостовов Дмитрий Сергеевич**
- Группа: **ФТ-303**
- ОС: Windows 11 с WSL2
- Архитектура: `amd64`

## Опыты

### 1
было
```bash
22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl -n homework-2 get pods -l app=feedback-quiz -o custom-columns='NAME:.metadata.name,UID:.me
tadata.uid,READY:.status.containerStatuses[0].ready,RESTARTS:.status.contain
erStatuses[0].restartCount'
NAME                            UID                                    READY   RESTARTS
feedback-quiz-df9ccd99d-48w7r   1f40e9b8-4e62-4d38-a4b8-247528f91b26   true    0
feedback-quiz-df9ccd99d-4q6w7   87b086e1-3b11-4698-b110-acabb4ebffbe   true    0
feedback-quiz-df9ccd99d-b2cws   4cc5ba1c-0f53-4745-b705-dba210401302   true    0
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$
```
стало
```bash
NAME                            UID                                    READY   RESTARTS
feedback-quiz-df9ccd99d-48w7r   1f40e9b8-4e62-4d38-a4b8-247528f91b26   true    2
feedback-quiz-df9ccd99d-4q6w7   87b086e1-3b11-4698-b110-acabb4ebffbe   true    0
feedback-quiz-df9ccd99d-b2cws   4cc5ba1c-0f53-4745-b705-dba210401302   true    0
```
client логи не менялмсь

```bash
2026-10-30T07:12:03.8109824+00:00 GET http://feedback-quiz.homework-2.svc.cluster.local/ -> 200 pod={"message":"Feedback quiz configuration was changed","author":"Dima","tokenConfigured":true,"podName":"feedback-quiz-df9ccd99d-48w7r"}
2026-10-30T07:12:05.8083749+00:00 GET http://feedback-quiz.homework-2.svc.cluster.local/ -> 200 pod={"message":"Feedback quiz configuration was changed","author":"Dima","tokenConfigured":true,"podName":"feedback-quiz-df9ccd99d-b2cws"}
```


запросы:

```bash
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ date -u
Fri Oct 30 07:06:10 UTC 2026
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Fri, 30 Oct 2026 07:06:11 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"message":"Feedback quiz configuration was changed","author":"Dima","tokenConfigured":true,"podName":"feedback-quiz-df9ccd99d-48w7r"}dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcacurl -i http://localhost:8082/livec867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/live
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:06:20 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/ready
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:06:27 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/you-are-death
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:06:33 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/live

HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:07:06 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/you-are-death
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:07:16 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/live

HTTP/1.1 503 Service Unavailable
Content-Type: application/problem+json; charset=utf-8
Date: Fri, 30 Oct 2026 07:07:21 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"type":"https://tools.ietf.org/html/rfc9110#section-15.6.4","title":"Service Unavailable","status":503,"traceId":"00-42cbe00a67a8400a36d3fa26f3669ada-019b13b921ed187d-00"}dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl -n homework-2 get pods -l app=feedback-quiz -w
NAME                            READY   STATUS    RESTARTS      AGE
feedback-quiz-df9ccd99d-48w7r   1/1     Running   2 (57s ago)   12h
feedback-quiz-df9ccd99d-4q6w7   1/1     Running   0             12h
feedback-quiz-df9ccd99d-b2cws   1/1     Running   0             12h
```

Перезапуск аосле запроса death происходил может секунд 20.   uuid сохранился ведь kuber не удалил под, а перезапустил контейнер внутри него. Так как увидел что livenessProbe упала.   Client получает запросы без ошибок т.к. Service отправил их на другие реплики

### 2
```bash
IP:               10.244.0.8
IPs:
  IP:           10.244.0.8
  Every 2.0s: kubectl -n homewor...  DESKTOP-A65H6VF: Fri Oct 30 12:41:38 2026

NAME                            READY   STATUS    RESTARTS        AGE
feedback-quiz-df9ccd99d-48w7r   1/1     Running   3 (7m43s ago)   13h
feedback-quiz-df9ccd99d-4q6w7   1/1     Running   0               13h
```

```bash
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ date -u
Fri Oct 30 07:48:26 UTC 2026
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/not-ready
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:48:35 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/ready
HTTP/1.1 503 Service Unavailable
Content-Type: application/problem+json; charset=utf-8
Date: Fri, 30 Oct 2026 07:48:41 GMT
Server: Kestrel
Transfer-Encoding: chunked
736909073c02491-00"}dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867curl -i http://localhost:8082/live/localhost:8082/live
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:48:54 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/

HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Fri, 30 Oct 2026 07:49:07 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"message":"Feedback quiz configuration was changed","author":"Dima","tokenConfigured":true,"podName":"feedback-quiz-df9ccd99d-48w7r"}dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f13695curl -i http://localhost:8082/ready$ curl -i http://localhost:8082/ready
HTTP/1.1 503 Service Unavailable
Content-Type: application/problem+json; charset=utf-8
Date: Fri, 30 Oct 2026 07:51:18 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"type":"https://tools.ietf.org/html/rfc9110#section-15.6.4","title":"Service Unavailable","status":503,"traceId":"00-ebd74c661dd839852879591a789b734d-2f3038edd889e53c-00"}dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/ready-again
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:52:41 GMT
Server: Kestrel

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ curl -i http://localhost:8082/ready
HTTP/1.1 200 OK
Content-Length: 0
Date: Fri, 30 Oct 2026 07:52:48 GMT
Server: Kestrel
```

после:
```bash
NAME                            READY   STATUS    RESTARTS      AGE   IP
       NODE                        NOMINATED NODE   READINESS GATES
feedback-quiz-df9ccd99d-48w7r   1/1     Running   3 (32m ago)   13h   10.244
.0.8   hosting-lab-control-plane   <none>           <none>
```
Readiness проба не отвечает за перезапуск. - только за то можно ли направить трафик пользователей на него или нет.
клиент продолжил получать хорошие сообщения - сервис перенаправил на другие реплики. 

### 3

начало:
```bash
Every 2.0s: kubectl -n homewor...  DESKTOP-A65H6VF: Fri Oct 30 13:09:32 2026

NAME                            READY   STATUS    RESTARTS      AGE   IP
       NODE                        NOMINATED NODE   READINESS GATES
feedback-quiz-df9ccd99d-48w7r   1/1     Running   3 (35m ago)   13h   10.244
.0.8   hosting-lab-control-plane   <none>           <none>
feedback-quiz-df9ccd99d-4q6w7   1/1     Running   0             13h   10.244
.0.9   hosting-lab-control-plane   <none>           <none>
feedback-quiz-df9ccd99d-b2cws   1/1     Running   0             13h   10.244
.0.7   hosting-lab-control-plane   <none>           <none>

kubectl -n homework-2 get replicasets -l app=feedback-quiz
NAME                      DESIRED   CURRENT   READY   AGE
feedback-quiz-df9ccd99d   3         3         3       13h


NAME                            UID                                    IMAGE                                  READY   RESTARTS
feedback-quiz-df9ccd99d-48w7r   1f40e9b8-4e62-4d38-a4b8-247528f91b26   localhost:5001/dima-feedback-quiz:v3   True    3
feedback-quiz-df9ccd99d-4q6w7   87b086e1-3b11-4698-b110-acabb4ebffbe   localhost:5001/dima-feedback-quiz:v3   True    0
feedback-quiz-df9ccd99d-b2cws   4cc5ba1c-0f53-4745-b705-dba210401302   localhost:5001/dima-feedback-quiz:v3   True    0
```


```bash
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kind load docker-image localhost:5001/dima-feedback-quiz:v4 --name hosting-lab
Image: "localhost:5001/dima-feedback-quiz:v4" with ID "sha256:e6f7fd7373739e796f789b8f41b9e8e557ef0c0964e8e0cf68f99aad9e621da0" not yet present on node "hosting-lab-control-plane", loading...
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ date -u
Fri Oct 30 08:19:37 UTC 2026
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl apply -f deployment.yaml
error: the path "deployment.yaml" does not exist

обновляю:

dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl -n homework-2 set image deployment/feedback-quiz \
  feedback-quiz=localhost:5001/dima-feedback-quiz:v4
deployment.apps/feedback-quiz image updated


dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl -n homework-2 get pods -l app=feedback-quiz -o wide
NAME                             READY   STATUS    RESTARTS   AGE    IP            NODE                        NOMINATED NODE   READINESS GATES
feedback-quiz-5569b89fd5-74s4t   1/1     Running   0          119s   10.244.0.13   hosting-lab-control-plane   <none>           <none>
feedback-quiz-5569b89fd5-cqfl8   1/1     Running   0          78s    10.244.0.15   hosting-lab-control-plane   <none>           <none>
feedback-quiz-5569b89fd5-hpjlk   1/1     Running   0          100s   10.244.0.14   hosting-lab-control-plane   <none>           <none>
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl -n homework-2 get replicasets -l app=feedback-quiz
NAME                       DESIRED   CURRENT   READY   AGE
feedback-quiz-5569b89fd5   3         3         3       2m7s
feedback-quiz-df9ccd99d    0         0         0       14h
dima@DESKTOP-A65H6VF:/mnt/wsl/docker-desktop-bind-mounts/Ubuntu/f6f12dcae8ba22ea47d0a91f1369525a65610ae391c867c2b4a4c0ddab35897a$ kubectl -n homework-2 rollout status deployment/feedback-quiz --timeout=180s
deployment "feedback-quiz" successfully rolled out
```

```bash
dima@DESKTOP-A65H6VF:/mnt/d/Dima/3University/DevOps/hw2/first$ kubectl -n homework-2 get deployment feedback-quiz -w
NAME            READY   UP-TO-DATE   AVAILABLE   AGE
feedback-quiz   3/3     3            3           14h
feedback-quiz   3/3     3            3           14h
feedback-quiz   3/3     3            3           14h
feedback-quiz   3/3     0            3           14h
feedback-quiz   3/3     1            3           14h
feedback-quiz   4/3     1            3           14h
feedback-quiz   4/3     1            4           14h
feedback-quiz   3/3     2            3           14h
feedback-quiz   4/3     2            3           14h
feedback-quiz   4/3     2            4           14h
feedback-quiz   3/3     2            3           14h
feedback-quiz   3/3     3            3           14h
feedback-quiz   4/3     3            3           14h
feedback-quiz   4/3     3            4           14h
feedback-quiz   3/3     3            3           14h
```

стали другие имена:
```bash
feedback-quiz-5569b89fd5-74s4t   1/1     Running   0          119s   10.244.0.13   hosting-lab-control-plane   <none>           <none>
feedback-quiz-5569b89fd5-cqfl8   1/1     Running   0          78s    10.244.0.15   hosting-lab-control-plane   <none>           <none>
feedback-quiz-5569b89fd5-hpjlk   1/1     Running   0          100s   10.244.0.14   hosting-lab-control-plane   <none>           <none>
```
старый ReplicaSet имел имя feedback-quiz-df9ccd99d
Стал другим:
NAME                       DESIRED   CURRENT   READY   AGE
feedback-quiz-5569b89fd5   3         3         3       2m7s
feedback-quiz-df9ccd99d    0         0         0       14h


В процессе обновления создавались 4-тые дополнительные реплики, потом удалялись (я с 3-мя работаю)

создавались 3 новые поды с новой v4 версией которые заменяли старые. maxSurge: 1 - разрешила создать максимум одну доп. Поэтому у клиента все работало.