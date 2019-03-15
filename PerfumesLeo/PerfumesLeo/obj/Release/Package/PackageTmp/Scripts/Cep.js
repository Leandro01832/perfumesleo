jQuery(function () {
    $("input[name='txtCep']").change(function () {
        var cep_code = $(this).val();
        if (cep_code.length <= 0) return;

        $.get("http://apps.widenet.com.br/busca-cep/api/cep.json", { code: cep_code }, function (result) {
            if (result.status != 1) {
                alert(result.message || "Cep não encontrado!!");
                return;
            }

            $("input[name='Estado']").val(result.state);
            $("input[name='Cidade']").val(result.city);
            $("input[name='Bairro']").val(result.district);
            $("input[name='Rua']").val(result.address);
            $("input[name='txtCep']").val(result.code);
            $("input[name='Cep']").val(result.code);

        });
    });
});