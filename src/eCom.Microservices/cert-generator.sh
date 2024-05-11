#!/bin/bash

# Directory for certificates
CERT_DIR=".cert"
mkdir -p ${CERT_DIR}

# Step 1: Create the CA
# Generating private key for the CA
openssl genrsa -out ${CERT_DIR}/ca.key 2048

# Generating the CA certificate
openssl req -x509 -new -nodes -key ${CERT_DIR}/ca.key -sha256 -days 1024 -out ${CERT_DIR}/ca.crt -subj "/C=US/ST=New York/L=New York/O=eComDev Inc/OU=IT/CN=My CA"

# Step 2: Create the wildcard certificate for *.ecom.dev
# Generating private key for the wildcard certificate
openssl genrsa -out ${CERT_DIR}/wildcard.key 2048

# Creating a certificate signing request (CSR) for the wildcard certificate
openssl req -new -key ${CERT_DIR}/wildcard.key -out ${CERT_DIR}/wildcard.csr -subj "/C=US/ST=New York/L=New York/O=eComDev Inc/OU=IT/CN=*.ecom.dev"

# Signing the wildcard certificate with the CA
openssl x509 -req -in ${CERT_DIR}/wildcard.csr -CA ${CERT_DIR}/ca.crt -CAkey ${CERT_DIR}/ca.key -CAcreateserial -out ${CERT_DIR}/wildcard.crt -days 500 -sha256

# Step 3: Create the client certificate
# Generating private key for the client certificate
openssl genrsa -out ${CERT_DIR}/client.key 2048

# Creating a certificate signing request (CSR) for the client certificate
openssl req -new -key ${CERT_DIR}/client.key -out ${CERT_DIR}/client.csr -subj "/C=US/ST=New York/L=New York/O=eComDev Inc/OU=Client/CN=Client Certificate"

# Signing the client certificate with the CA
openssl x509 -req -in ${CERT_DIR}/client.csr -CA ${CERT_DIR}/ca.crt -CAkey ${CERT_DIR}/ca.key -CAcreateserial -out ${CERT_DIR}/client.crt -days 500 -sha256

echo "Certificate creation complete. Files are located in ${CERT_DIR}"
