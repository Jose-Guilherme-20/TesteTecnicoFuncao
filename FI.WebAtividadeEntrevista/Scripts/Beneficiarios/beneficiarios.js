let beneficiarios = [];

$(document).ready(function () {
    aplicarMascaras();

    $('#modalBeneficiarios').on('shown.bs.modal', function () {
        aplicarMascaras();
    });

    $('#btnIncluirBeneficiario').click(function () {
        const cpf = $('#CpfBeneficiario').val().trim();
        const nome = $('#NomeBeneficiario').val().trim();

        if (!cpf || !nome) {
            alert("Preencha CPF e Nome.");
            return;
        }

        if (!validarCpf(cpf)) {
            alert("CPF inválido.");
            return;
        }

        if (beneficiarios.some(b => b.Cpf === cpf)) {
            alert("Este CPF já foi adicionado.");
            return;
        }

        if (beneficiarios.length >= 5) {
            alert("Máximo de 5 beneficiários permitido.");
            return;
        }

        beneficiarios.push({ Cpf: cpf, Nome: nome });
        atualizarTabelaBeneficiarios();

        $('#formBeneficiario')[0].reset();
    });
});

function atualizarTabelaBeneficiarios() {
    const tbody = $("#tabelaBeneficiarios tbody");
    tbody.empty();

    beneficiarios.forEach((b, i) => {
        tbody.append(`
            <tr>
                <td>${b.Cpf}</td>
                <td>${b.Nome}</td>
                <td>
                    <button type="button" class="btn btn-danger btn-sm" onclick="removerBeneficiario(${i})">Remover</button>
                </td>
            </tr>
        `);
    });

    $('#BeneficiariosJson').val(JSON.stringify(beneficiarios));
}

function removerBeneficiario(index) {
    beneficiarios.splice(index, 1);
    atualizarTabelaBeneficiarios();
}

function aplicarMascaras() {
    $('.cpf').mask('000.000.000-00');
    $('.cep').mask('00000-000');
    $('.telefone').mask('(00) 0000-0000');
}

function validarCpf(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');
    if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) return false;

    let soma = 0, resto;
    for (let i = 1; i <= 9; i++)
        soma += parseInt(cpf.substring(i - 1, i)) * (11 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.charAt(9))) return false;

    soma = 0;
    for (let i = 1; i <= 10; i++)
        soma += parseInt(cpf.substring(i - 1, i)) * (12 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    return resto === parseInt(cpf.charAt(10));
}