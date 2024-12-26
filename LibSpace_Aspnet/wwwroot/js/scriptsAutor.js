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

function submitPaisForm() {
    const nomePais = document.getElementById("NomePais").value;
    const validationMessage = document.getElementById("nomePaisValidation");

    validationMessage.textContent = ""; // Limpa mensagens de erro

    fetch('/Autors/Createpais', {
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