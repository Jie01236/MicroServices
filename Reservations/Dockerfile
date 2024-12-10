#Stage 1
FROM python:3.10-slim as build

WORKDIR /app

#Install with apt
RUN apt-get update && apt-get install -y --no-install-recommends \
python3 python3-venv python3-pip golang nginx curl sqlite3  -y \
&& rm -rf /var/lib/apt/lists/*



#PIP requirements
COPY requirements.txt requirements.txt



#RUN pip install --no-cache-dir -r requirements.txt
RUN pip install --upgrade pip && pip install --prefix=/install -r requirements.txt




#Stage 2
FROM python:3.10-slim as runtime

WORKDIR /app

#Copy depedencies
COPY --from=build /install /usr/local

#Copy app files
COPY . .



#Expose port
EXPOSE 8000


#Launch app
CMD [ "python3", "app.py" ]
