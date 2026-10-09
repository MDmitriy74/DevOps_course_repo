import os
import redis
import psycopg2
from flask import Flask, request, Response

app = Flask(__name__)

cache = redis.Redis(host=os.environ.get("REDIS_HOST", "cache"), port=6379, decode_responses=True)

def get_db_connection():
    return psycopg2.connect(
        host=os.environ.get("DB_HOST", "db"),
        database=os.environ.get("POSTGRES_DB", "notes_db"),
        user=os.environ.get("POSTGRES_USER", "user"),
        password=os.environ.get("POSTGRES_PASSWORD", "password")
    )

def init_db():
    conn = get_db_connection()
    cur = conn.cursor()
    cur.execute("CREATE TABLE IF NOT EXISTS notes (id SERIAL PRIMARY KEY, text TEXT NOT NULL);")
    conn.commit()
    cur.close()
    conn.close()

init_db()

@app.get("/")
def index():
    visits = cache.incr("visits")
    return f"Docker Compose demo222222 - visits: {visits}\n"

@app.get("/notes")
def get_notes():
    conn = get_db_connection()
    cur = conn.cursor()
    cur.execute("SELECT text FROM notes;")
    notes = [row[0] for row in cur.fetchall()]
    cur.close()
    conn.close()
    return "\n".join(notes) if notes else "No notes"

@app.get("/notes/add")
def add_note():
    text = request.args.get("text", "")
    if not text:
        return "Text is required", 400
    conn = get_db_connection()
    cur = conn.cursor()
    cur.execute("INSERT INTO notes (text) VALUES (%s);", (text,))
    conn.commit()
    cur.close()
    conn.close()
    return f"Added: {text}", 201

if __name__ == "__main__":
    debug_mode = os.environ.get("FLASK_DEBUG", "0") == "1"
    app.run(host="0.0.0.0", port=8000, debug=debug_mode)