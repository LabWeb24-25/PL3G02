// Envia o formulário de criação de País via AJAX
function submitPaisForm() {
    const nomePais = document.getElementById("NomePais").value;
    const validationMessage = document.getElementById("nomePaisValidation");

    validationMessage.textContent = ""; // Limpa mensagens de erro

    fetch('/Livroes/Createpais', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() // CSRF Token
        },
        body: JSON.stringify({ NomePais: nomePais })
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                return response.json().then(error => { throw error; });
            }
        })
        .then(data => {
            // Fecha o modal e atualiza a lista de países
            $('#createPaisModal').modal('hide');
            const select = document.querySelector('select[name="IdLingua"]');
            const option = new Option(data.nome, data.id, false, true);
            select.add(option);
        })
        .catch(error => {
            if (error.errors && error.errors.NomePais) {
                validationMessage.textContent = error.errors.NomePais;
            } else {
                alert("Erro ao criar país. Tente novamente.");
            }
        });
}
function submitGeneroForm() {
    const nomeGeneroInput = document.querySelector("#createGeneroModal #NomeGenero"); // Garante que pegue o input dentro do modal
    const nomeGenero = nomeGeneroInput?.value;
    const validationMessage = document.querySelector("#createGeneroModal #nomeGeneroValidation");

    validationMessage.textContent = ""; // Limpa mensagens de erro

    if (!nomeGenero || nomeGenero.length > 20) {
        validationMessage.textContent = "Nome do Gênero é obrigatório e deve ter no máximo 20 caracteres.";
        return;
    }

    // Capturar o token CSRF gerado pelo @Html.AntiForgeryToken()
    const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]')?.value || "";

    fetch('/Livroes/CreateGenero', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': csrfToken // Token CSRF
        },
        body: JSON.stringify(nomeGenero) // Envia apenas a string
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                return response.json().then(error => { throw error; });
            }
        })
        .then(data => {
            // Fecha o modal e atualiza a lista de gêneros
            $('#createGeneroModal').modal('hide');
            const select = document.querySelector('select[name="IdGeneros"]');
            const option = new Option(data.nome, data.id, false, true);
            select.add(option);
        })
        .catch(error => {
            if (error?.errors) {
                validationMessage.textContent = error.errors;
            } else {
                alert("Erro ao criar Gênero. Tente novamente.");
            }
        });
}

function updateSelects(paises) {
    const selects = document.querySelectorAll('select[name="IdLingua"]');
    selects.forEach(select => {
        // Limpa as opções existentes
        select.innerHTML = '';
        // Adiciona as novas opções
        paises.forEach(pais => {
            const option = new Option(pais.nome, pais.id, false, false);
            select.add(option);
        });
    });
}

function submitPaisAutorForm() {
    const nomePais = document.getElementById("NomePais").value;
    const validationMessage = document.getElementById("nomePaisValidation");

    validationMessage.textContent = ""; // Limpa mensagens de erro

    fetch('/Livroes/Createpais', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() // CSRF Token
        },
        body: JSON.stringify({ NomePais: nomePais })
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                return response.json().then(error => { throw error; });
            }
        })
        .then(data => {
            // Fecha o modal
            $('#createPaisAutorModal').modal('hide');

            // Atualiza todos os selects do país
            updateSelects(data.paises);

            // Define a seleção apenas no pop-up do Autor
            const autorSelect = document.getElementById('autorPaisSelect');
            if (autorSelect) {
                autorSelect.value = data.id;
            } else {
                console.error('Select do modal de Autor não encontrado.');
            }
        })
        .catch(error => {
            if (error.errors && error.errors.NomePais) {
                validationMessage.textContent = error.errors.NomePais;
            } else {
                alert("Erro ao criar país. Tente novamente.");
            }
        });
}

function submitAutorForm() {
    // Obtém os valores dos campos do formulário
    const nomeAutor = document.getElementById('NomeAutor').value;
    const pseudonimo = document.getElementById('Pseudonimo').value;
    const dataNascimento = document.getElementById('DataNascimento').value;
    const bibliografia = document.getElementById('Bibliografia').value;

    const validationMessages = {
        NomeAutor: document.getElementById("nomeAutorValidation"),
        Pseudonimo: document.getElementById("pseudonimoValidation"),
        DataNascimento: document.getElementById("dataNascimentoValidation"),
        Bibliografia: document.getElementById("bibliografiaValidation"),
    };

    // Limpa mensagens de erro
    Object.values(validationMessages).forEach(msg => (msg.textContent = ""));

    // Monta o corpo da requisição manualmente
    const requestBody = new URLSearchParams();
    requestBody.append("nomeAutor", nomeAutor);
    requestBody.append("pseudonimo", pseudonimo);
    requestBody.append("dataNascimento", dataNascimento);
    requestBody.append("bibliografia", bibliografia);

    fetch('/Livroes/CreateAutor', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(), // CSRF Token
        },
        body: requestBody.toString(),
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                return response.json().then(error => {
                    throw error;
                });
            }
        })
        .then(data => {
            // Fecha o modal e atualiza a lista de autores
            $('#createAutorModal').modal('hide');

            const select = document.querySelector('select[name="IdAutor"]');
            const option = new Option(data.nome, data.id, false, true);
            select.add(option);
            select.value = data.id; // Seleciona automaticamente o novo autor
        })
        .catch(error => {
            if (error.errors) {
                // Exibe mensagens de erro específicas
                Object.entries(error.errors).forEach(([key, messages]) => {
                    if (validationMessages[key]) {
                        validationMessages[key].textContent = messages.join(', ');
                    }
                });
            } else {
                alert("Erro ao criar autor. Tente novamente.");
            }
        });
}



// Envia o formulário de criação de Editora via AJAX  FALTA CONTROLLER
function submitEditoraForm() {
    // Obtém os valores dos campos do formulário
    const formData = new FormData(document.getElementById('createEditoraForm'));
    const validationMessages = {
        NomeEditora: document.getElementById("nomeEditoraValidation"),
        InfoEditora: document.getElementById("infoEditoraValidation"),
    };

    // Limpa mensagens de erro
    Object.values(validationMessages).forEach(msg => (msg.textContent = ""));

    // Converte o FormData para URLSearchParams (formato necessário para x-www-form-urlencoded)
    const requestBody = new URLSearchParams();
    formData.forEach((value, key) => {
        requestBody.append(key, value);
    });

    fetch('/Livroes/CreateEditora', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(), // CSRF Token
        },
        body: requestBody.toString(),
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                return response.json().then(error => {
                    throw error;
                });
            }
        })
        .then(data => {
            // Fecha o modal e atualiza a lista de editoras
            $('#createEditoraModal').modal('hide');
            const select = document.querySelector('select[name="IdEditora"]');
            const option = new Option(data.nome, data.id, false, true);
            select.add(option);
        })
        .catch(error => {
            if (error.errors) {
                // Exibe mensagens de erro específicas
                Object.entries(error.errors).forEach(([key, messages]) => {
                    if (validationMessages[key]) {
                        validationMessages[key].textContent = messages.join(', ');
                    }
                });
            } else {
                alert("Erro ao criar editora. Tente novamente.");
            }
        });
}

function previewImage(event, previewId) {
    const input = event.target;
    const preview = document.getElementById(previewId);

    if (input.files && input.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = 'block'; // Exibe a imagem no preview
            document.getElementById('removeImageButton').style.display = 'block';
        };

        reader.readAsDataURL(input.files[0]);
    } else {
        preview.src = '';
        preview.style.display = 'none';
        document.getElementById('removeImageButton').style.display = 'none';
    }
}

function removeImage(inputId, previewId, buttonId) {
    const input = document.getElementById(inputId);
    const preview = document.getElementById(previewId);
    const button = document.getElementById(buttonId);

    input.value = ''; // Limpa o input
    preview.src = ''; // Remove o src da imagem
    preview.style.display = 'none'; // Esconde o preview
    button.style.display = 'none'; // Esconde o botão
    if (statusInput != null) {
        statusInput.value = 1; // Define o valor para 1, indicando que a imagem foi removida
    }
}