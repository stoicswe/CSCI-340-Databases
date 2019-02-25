from sqalchmey import create_engine
import pandas

engine = create_engine('postgresql://cslab:TacoSh%40ck@localhost:5432/cooking')
con = engine.connect()
result = con.execute("SELECT (id,score) FROM posts ORDER BY score DESC;")
print(result)