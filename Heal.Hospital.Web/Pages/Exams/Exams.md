# Tela de Lista de Exames — Documentação de Funcionalidades

Página principal de gestão de exames de imageamento do St. Mungus. O comportamento varia conforme o papel do usuário: a **Recepcionista** tem acesso a uma grade avançada (Tabulator) com filtros dinâmicos, indicadores visuais e conjunto completo de ações; os demais papéis veem uma tabela simplificada com busca server-side e paginação.

---

## 1. Toolbar — cabeçalho da seção

Presente para todos os papéis.

| Elemento                 | Descrição                                                                                                                                                                                                                                                                                   |
| ------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Título "Lista de Exames" | Nome da seção com contador de exames visíveis no badge ao lado.                                                                                                                                                                                                                             |
| **Atualizar lista**      | Botão no canto superior direito. Para a Recepcionista: recarrega os dados na grade Tabulator sem recarregar a página (chama `E.refreshAll()`). Para os demais papéis: recarrega a página completa (`window.location.reload()`). O ícone de seta gira enquanto a operação está em andamento. |

---

## 2. Painel de filtros (somente Recepcionista)

Painel colapsável ativado pelo cabeçalho "Filtros". Um badge indica quantos filtros estão aplicados no momento.

### Campos de filtro disponíveis

| Campo                        | Tipo        | Descrição                                                                                                                                            |
| ---------------------------- | ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID Paciente                  | Texto livre | Filtra pelo código do paciente (ex.: `PAC-00012`).                                                                                                   |
| Nome do paciente             | Texto livre | Busca parcial, sem distinção de maiúsculas.                                                                                                          |
| Nº Acesso                    | Texto livre | Número de acesso do exame.                                                                                                                           |
| Médico executor              | Select      | Lista dinâmica dos médicos presentes nos dados.                                                                                                      |
| Unidade                      | Select      | Lista dinâmica das unidades presentes nos dados.                                                                                                     |
| Modalidade                   | Select      | Lista dinâmica. Ao selecionar uma modalidade, o campo Procedimento é habilitado automaticamente e filtrado para os procedimentos daquela modalidade. |
| Procedimento                 | Select      | Dependente da Modalidade selecionada (desabilitado enquanto nenhuma modalidade estiver selecionada).                                                 |
| Tipo                         | Select      | PS / Int. / Eletivo.                                                                                                                                 |
| Situação                     | Select      | Liberação / Disponível / Laudando / Pendência / Revisão / Assinatura / Aprovado / Cancelado.                                                         |
| Estudo — início / fim        | Date range  | Filtra pelo intervalo de data do estudo.                                                                                                             |
| Finalização — início / fim   | Date range  | Filtra pelo intervalo de data de finalização do laudo.                                                                                               |
| Emergências                  | Checkbox    | Exibe somente exames marcados como emergência.                                                                                                       |
| Pendências                   | Checkbox    | Exibe somente exames com pendência aberta.                                                                                                           |
| Achado crítico não resolvido | Checkbox    | Exibe somente exames com achado crítico pendente de contato.                                                                                         |
| Achado crítico resolvido     | Checkbox    | Exibe somente exames com achado crítico já resolvido.                                                                                                |
| Arquivados                   | Checkbox    | Exibe somente exames em cold storage.                                                                                                                |
| Atrasados                    | Checkbox    | Exibe somente exames com SLA expirado.                                                                                                               |
| Próximos da expiração        | Checkbox    | Exibe somente exames cujo prazo de laudo está próximo de vencer.                                                                                     |

**Botões de ação do painel:** "Limpar" (reseta todos os campos) e "Filtrar" (aplica os critérios à grade).

A grade exibe um aviso em destaque quando há filtros ativos, lembrando que exames podem estar ocultos.

---

## 3. Legenda de situações e indicadores (somente Recepcionista)

Linha de referência visual exibida acima da grade com dois grupos:

- **Situação:** badges coloridos para cada status possível do laudo — Liberação, Disponível, Laudando, Pendência, Revisão, Assinatura, Aprovado, Cancelado.
- **Indicadores:** ícones que aparecem na coluna ID Paciente para sinalizar estados especiais do exame (ver seção 5).

---

## 4. Chips de SLA — Atrasados e Próximos da expiração (somente Recepcionista)

Duas pílulas compactas exibidas acima da grade quando há exames fora do prazo ou próximos do vencimento. Cada chip mostra a contagem e, ao clicar, aplica automaticamente o filtro correspondente na grade (atalho para "Atrasados" ou "Próximos da expiração").

---

## 5. Grade de exames — Recepcionista (Tabulator)

Grade dinâmica com paginação local (padrão 10 itens), ordenação por coluna, e detalhe expandido por linha.

### Colunas

| Coluna           | Conteúdo                                                                                        |
| ---------------- | ----------------------------------------------------------------------------------------------- |
| **ID Paciente**  | Código do paciente + indicadores de estado (ícones clicáveis — ver abaixo) + detalhe expandido. |
| **Paciente**     | Nome completo.                                                                                  |
| **Sexo**         | M / F / Outro.                                                                                  |
| **AN**           | Número de acesso do exame (código compacto).                                                    |
| **Mod.**         | Modalidade com badge colorido por tipo (DX, CT, MR, etc.) e contador de imagens.                |
| **Procedimento** | Descrição do procedimento.                                                                      |
| **Situação**     | Badge de status do laudo.                                                                       |
| **Tipo**         | Badge PS / Int. / Eletivo.                                                                      |
| **Ações**        | Botões de ação por linha (ver seção 6).                                                         |

### Detalhe expandido (sub-linha)

Cada linha pai exibe uma sub-linha com informações adicionais:

- Nome social, data de nascimento, idade, ID do estudo, data do estudo, data de liberação, data do laudo.
- Unidade, técnico responsável, médico executor, médico solicitante.

### Indicadores na coluna ID Paciente

Ícones clicáveis que abrem modais específicos:

| Ícone         | Significado                    | Ação                                         |
| ------------- | ------------------------------ | -------------------------------------------- |
| ⚠️ laranja    | Achado crítico não resolvido   | Abre modal de Achado Crítico — Não Resolvido |
| ✅ verde      | Achado crítico resolvido       | Abre modal de Achado Crítico — Histórico     |
| 🚩            | Pendência aberta               | Abre modal de Pendência                      |
| 📎 + contagem | Documentos/anexos              | Abre modal de Download                       |
| 🗃️            | Exame arquivado (cold storage) | Abre modal de Itens Arquivados               |
| 🔑            | Código de download gerado      | Abre modal de Código de Download             |
| ☁️            | Laudo baixado pelo paciente    | Abre modal de Histórico de Downloads         |
| ⏱ vermelho    | Laudo atrasado                 | Abre modal de SLA                            |
| ⏱ laranja     | Laudo próximo da expiração     | Abre modal de SLA                            |

---

## 6. Ações por linha

### Recepcionista (grade Tabulator)

| Botão                      | Ícone                           | Ação                                                |
| -------------------------- | ------------------------------- | --------------------------------------------------- |
| Editar dados do paciente   | ✏️ `bi-pencil-square`           | Abre o modal de edição do paciente.                 |
| Baixar laudos e anexos     | ⬇️ `bi-download`                | Abre o modal de download do exame.                  |
| Imprimir laudo             | 🖨️ `bi-printer`                 | Disponível somente se o exame tiver laudo aprovado. |
| Reencaminhar imagens DICOM | 🖧 `bi-hdd-network` (amarelo)    | Abre o modal de envio DICOM.                        |
| Atualizar exame            | 🔄 `bi-arrow-clockwise` (cinza) | Simula a atualização individual do exame na grade.  |

### Outros papéis (tabela legacy)

| Botão                      | Ícone                           | Ação                                                           |
| -------------------------- | ------------------------------- | -------------------------------------------------------------- |
| Ver detalhes               | 👁️ `bi-eye`                     | Placeholder — abre detalhe do exame.                           |
| Ver laudo                  | 📄 `bi-file-earmark-text`       | Disponível somente se o exame tiver laudo; abre link do laudo. |
| Reencaminhar imagens DICOM | 🖧 `bi-hdd-network` (amarelo)    | Abre o modal de envio DICOM.                                   |
| Atualizar exame            | 🔄 `bi-arrow-clockwise` (cinza) | Simula a atualização individual da linha na tabela.            |

---

## 7. Modais de ação

### 7.1 Editar dados do paciente (Recepcionista)

Abre ao clicar no botão de lápis na coluna Ações. Modal em duas abas:

- **Dados do Paciente:** formulário com nome completo, nome social, nome da mãe, sexo, telefone, data/hora de nascimento, CPF, e-mail, data/hora do estudo e código de acesso. Alterações são propagadas (mock) para dados do paciente, tags DICOM e cabeçalho do laudo.
- **Histórico de alteração:** lista as alterações anteriores realizadas no cadastro do paciente.

---

### 7.2 Download do exame (Recepcionista)

Abre ao clicar no botão de download ou no indicador de documentos/annexos. Modal em duas abas:

- **Exame atual:** lista laudos disponíveis para download, arquivos anexados (com informações de tipo, tamanho, responsável) e seção de imagens DICOM com visualizador embutido (colapsável) com botão para abrir em nova aba.
- **Exames anteriores:** histórico de exames anteriores do mesmo paciente disponíveis para consulta.

---

### 7.3 Achado crítico — Não resolvido (Recepcionista)

Abre ao clicar no indicador ⚠️. Exibe:

- Data do achado, médico responsável e CRM.
- Mensagem de alerta do médico.
- Dados de contato do paciente (nome, telefone, e-mail).
- Campo de **registro de contato** (obrigatório) para documentar a comunicação com o paciente.
- Botão **Registrar contato** (salva a nota sem resolver) e **Marcar como resolvido** (fecha o achado).

---

### 7.4 Achado crítico — Histórico (Recepcionista)

Abre ao clicar no indicador ✅. Exibe o histórico completo do achado crítico resolvido: data, médico, mensagem original, registro dos contatos realizados e data de resolução.

---

### 7.5 Pendência do exame (Recepcionista)

Abre ao clicar no indicador 🚩. Exibe:

- Dados do médico que registrou a pendência e descrição.
- Seção **Sanar pendência:** campo para anexar arquivos e campo de resposta obrigatório.
- Botão **Enviar resposta** resolve a pendência (mock).

---

### 7.6 Itens arquivados / Cold storage (Recepcionista)

Abre ao clicar no indicador 🗃️. Lista os itens do exame em cold storage (nome, tipo, tamanho, data de arquivamento). Botão **Solicitar restauração** inicia o processo de recuperação com aviso de que pode levar algumas horas. Se um item específico estiver em recuperação, o modal de **Recuperação de arquivo** mostra detalhes e o botão de confirmação.

---

### 7.7 Código de download do laudo (Recepcionista)

Abre ao clicar no indicador 🔑. Exibe o código numérico que o paciente utiliza no portal para baixar os laudos, com botão de cópia para a área de transferência e a data de geração do código.

---

### 7.8 Histórico de downloads do paciente (Recepcionista)

Abre ao clicar no indicador ☁️. Exibe uma tabela com o registro de cada vez que o paciente baixou os laudos: data/hora, dispositivo e endereço IP.

---

### 7.9 SLA do laudo (Recepcionista)

Abre ao clicar nos indicadores de atraso ⏱. Modal em três abas:

- **Datas & SLA:** tabela com todas as datas-chave do exame (estudo, última imagem, liberação, início e finalização do laudo) e gauge circular mostrando o percentual do prazo consumido.
- **Fluxo entre etapas:** diagrama Sankey (Google Charts) mostrando o tempo gasto em cada transição do fluxo de laudo.
- **Linha do tempo:** gráfico de barras horizontais (Google Charts) mostrando as etapas do processo como uma linha do tempo.

---

### 7.10 Reencaminhar imagens DICOM (todos os papéis)

Disponível para todos os usuários via botão 🖧 na coluna Ações. Permite encaminhar as imagens de um estudo para outro servidor DICOM (teleradiodiagnóstico, C-MOVE).

**Cabeçalho do modal:** nome do paciente e ID do estudo selecionado.

**Modo de seleção do servidor destino:**

| Modo                    | Descrição                                                                                                                                         |
| ----------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Servidor Cadastrado** | Dropdown com os servidores DICOM registrados no sistema (nome + AE Title). Ao selecionar, exibe as informações do servidor: AE Title, IP e porta. |
| **Servidor Manual**     | Formulário com campos AE Title (máx. 16 chars), IP/Host e Porta (1–65535) para informar um servidor não cadastrado.                               |

**Ping / C-ECHO:** botão para testar a conectividade com o servidor antes do envio. Exibe resultado ("Servidor disponível" ou "Servidor indisponível") após ~1,2 s.

**Reencaminhar:** confirma o envio (mock, ~1,5 s). Fecha o modal e exibe toast de confirmação "Imagens reencaminhadas com sucesso."

---

## 8. Busca e filtro — Outros papéis

Formulário server-side acima da tabela com dois campos:

- **Buscar:** texto livre por nome do paciente ou número do exame.
- **Status:** select com Agendado / Em andamento / Concluído / Laudo pendente / Laudo emitido.

Botão **Filtrar** recarrega a página com os parâmetros via GET. A URL é compartilhável/marcável.

---

## 9. Paginação — Outros papéis

Paginação server-side abaixo da tabela, exibida somente quando há mais de uma página. Navegação por links (anterior / páginas numeradas / próxima). Exibe o total de exames e a página atual.

---

## 10. Tabela de exames — Outros papéis

Tabela HTML simples com as colunas: ID, Paciente (com avatar de iniciais), Modalidade, Médico, Data, Status e Ações. Não possui ordenação dinâmica nem detalhe expandido.

---

## 11. Notificações toast

Mensagens de feedback não-intrusivas exibidas no canto inferior da tela após operações assíncronas:

- Lista atualizada. _(refresh global)_
- Exame atualizado. _(refresh individual)_
- Imagens reencaminhadas com sucesso. _(envio DICOM)_
- Contato registrado com sucesso. _(achado crítico)_
- Resposta enviada para a pendência. _(pendência)_
- Restauração solicitada. _(cold storage)_
- Código copiado. _(código de download)_

---

## 12. Papéis e acesso

| Papel               |   Grade   | Filtros avançados | Editar paciente | Modais de indicadores | DICOM send | Row refresh |
| ------------------- | :-------: | :---------------: | :-------------: | :-------------------: | :--------: | :---------: |
| Receptionist        | Tabulator |        ✅         |       ✅        |          ✅           |     ✅     |     ✅      |
| RadiologyTechnician |  Legacy   |         —         |        —        |           —           |     ✅     |     ✅      |
| HospitalManager     |  Legacy   |         —         |        —        |           —           |     ✅     |     ✅      |
| HospitalFinancial   |  Legacy   |         —         |        —        |           —           |     ✅     |     ✅      |
| RequestingPhysician |  Legacy   |         —         |        —        |           —           |     ✅     |     ✅      |
| MedicalAuxiliary    |  Legacy   |         —         |        —        |           —           |     ✅     |     ✅      |
| Transcriptionist    |  Legacy   |         —         |        —        |           —           |     ✅     |     ✅      |
