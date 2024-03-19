# PostechFiapAluraFase3
Tech Challenge da Fase 3 do curso ARQUITETURA DE SISTEMAS .NET COM AZURE

# Sobre o Projeto

Este projeto foi desenvolvido durante a FASE 3 do curso ARQUITETURA DE SISTEMAS .NET COM AZURE, como base foi utilizado o projeto entregue na FASE 1. Desta vez o backend do projeto foi promovido a microserviços que ficam em execução na núvem.

Nesta configuração o intuito é exemplificar, de forma simplificada, o dinamismo de uma arquitetura em Micro Serviços, rodando em containers, hospedado em núvem.

Este projeto consiste na criação de três Microserviços, atuando como backend do projeto entregue no Tech Challenge da Fase 1.

## Api Consulta (PosTech.CadPac.Api)
O primeiro microserviço é uma api de consulta, o segundo serviço é uma api (producer) que dispara os eventos atráves do Azure Serviçe Bus para o terceiro microseriço realize a persistência dos dados.

## Microserviço Producer (Projeto PosTech.CadPac.Producer.Api)
É uma Api (producer) que está em execução em Instância de Container no Azure e que envia as mensagens para o Azure Service Bus.
Esta api necessita de autenticação!

## Microserviço Consumer (Projeto PosTech.CadPac.MicroService.Consumer)
Monitorando os eventos no Service Bus, temos um Microserviço (Worker) que recebe as mensagens e as processa, armazenando no banco de dados Mongodb em nuvem.

# 🔧 Como executar o projeto (Back End)

Para a execução dos MicroServiços, à partir do código disponibilizado no GIT é necessário ajustar a informação de conexão com o Azure App Configuration, local onde estão armazenadas as configurações necessárias e comuns aos Microserviços. Este recurso foi colocado em um Enviroment (um em cada microserviço) com nome postechazappconfiguration, e é através dele que as demais chaves necessárias ao funcionamento dos microserviços são adquiridas.

Os projeto da solução que representam os microserviços são:
1) Projeto PosTech.CadPac.Api 

Api de consulta, possui as rodas que realizam a consulta das informações em banco de dados. Estas rotas estão livres de autenticação.

2) Projeto PosTech.CadPac.Producer.Api (producer)
Esta api necessita de autenticação
	
- Arquivo Program.cs linha 12, informar o valor correto em
  .AddAzureAppConfiguration("CONNECTIONSTRING COM AZURE APP CONFIGURATION")

3) Projeto PosTech.CadPac.MicroService.Consumer (consumer)
- Arquivo Program.cs linha 12, informar o valor correto em
  .AddAzureAppConfiguration("CONNECTIONSTRING COM AZURE APP CONFIGURATION")

Para autenticação (geração do token jwt) com PosTech.CadPac.Api é necessário passar um clientId e um clientSecret
