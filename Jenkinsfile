pipeline {
    agent { label 'docker-server' }

    environment {
        DOCKER_IMAGE = 'payment-api:latest'
        CONTAINER_NAME = 'payment-api'
        APP_PORT = '11000' // Change if needed
        DOCKER_CREDENTIALS_ID = 'docker'
    }

    stages {
        stage('Build Docker Image') {
            steps {
                sh "docker build -t ${DOCKER_IMAGE} ."
            }
        }

        stage('Stop & Remove Old Container') {
            steps {
                sh """
                    docker stop ${CONTAINER_NAME} || true
                    docker rm ${CONTAINER_NAME} || true
                """
            }
        }

        stage('Run New Container') {
            steps {
                sh "docker run -d --name ${CONTAINER_NAME} -p ${APP_PORT}:80 ${DOCKER_IMAGE}"
            }
        }
    }

    post {
        success {
            echo '✅ Deployment successful on docker-server node!'
        }
        failure {
            echo '❌ Deployment failed.'
        }
    }
}
