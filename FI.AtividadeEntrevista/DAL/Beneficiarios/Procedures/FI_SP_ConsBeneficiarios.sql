﻿CREATE PROC FI_SP_ConsBeneficiarios
	@IDCLIENTE BIGINT
AS
BEGIN
	IF(ISNULL(@IDCLIENTE,0) = 0)
		SELECT NOME, CPF ID, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK)
	ELSE
		SELECT NOME, CPF,ID, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IDCLIENTE
END