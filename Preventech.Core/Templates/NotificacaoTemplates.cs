namespace Preventech.Core.Templates;

public static class NotificacaoTemplates
{
    public static class BoasVindas
    {
        public static string Titulo => "Bem-vindo!";
        
        public static string MensagemEmail(string nomeUsuario) =>
            $"""
            <html>
            <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">
            <div style="max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #2c3e50;">Olá {nomeUsuario},</h2>
                
                <p>Seja bem-vindo ao <strong>Preventech</strong>!</p>
                
                <p>Aqui você poderá:</p>
                <ul>
                <li>Requisitar serviços</li>
                <li>Acompanhar seus pedidos</li>
                <li>Organizar seu trabalho</li>
                </ul>
                
                <p>Obrigado por contar conosco nessa incrível jornada.</p>
                
                <hr style="border: none; border-top: 1px solid #ddd; margin: 20px 0;">
                
                <p>Se tiver qualquer dúvida, entre em contato com nossa equipe de suporte.</p>
                
                <p style="margin-top: 30px; color: #7f8c8d;">
                Atenciosamente,<br>
                <strong>Equipe Preventech</strong>
                </p>
            </div>
            </body>
            </html>
            """;

        public static string MensagemSite(string nomeUsuario) =>
            $"""
            Olá {nomeUsuario},

            Seja bem-vindo ao Preventech!

            Seu cadastro foi realizado com sucesso. Agora você pode acessar todas as funcionalidades do sistema.

            Se tiver qualquer dúvida, entre em contato com nossa equipe de suporte.
            
            Atenciosamente,
            Equipe Preventech
            """;
    }

    public static class OrdemServico
    {
        public static string TituloCriada(int numeroOS) => 
            $"Nova Ordem de Serviço";
        
        public static string MensagemCriada(int numeroOS, string equipamento) =>
            $"Uma nova ordem de serviço (ID: {numeroOS}) foi criada para o equipamento: {equipamento}.\nClique para ver os detalhes.";

        public static string TituloAtualizada(int numeroOS) => 
            $"Ordem de Serviço Atualizada";
        
        public static string MensagemAtualizada(int numeroOS, string status) =>
            $"A ordem de serviço (ID: {numeroOS}) teve seu status alterado para: {status}.";

        public static string TituloConcluida => $"Ordem de Serviço Concluída";

        public static string MensagemEmailConcluida(Models.OrdemServico os) =>
            $"""
            <html>
            <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">
            <div style="max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #2c3e50;">Olá {os.Requisitante!.Nome},</h2>
                
                <p>Sua Ordem de Serviço <strong>{os.Titulo}</strong> (ID: {os.Id}) foi concluída com sucesso pelo técnico {os.TecnicoResponsavel!.Nome}.</p>
                {(string.IsNullOrWhiteSpace(os.DescricaoSolucao) 
                    ? "" 
                    : $"<p><strong>Ações Realizadas:</strong><br>{os.DescricaoSolucao.Replace("\n", "<br>")}</p>")}
                
                <p>Abaixo, segue-se o anexo da ordem de serviço.</p>

                <hr style="border: none; border-top: 1px solid #ddd; margin: 20px 0;">
                
                <p>Se tiver qualquer dúvida, entre em contato com nossa equipe de suporte.</p>
                
                <p style="margin-top: 30px; color: #7f8c8d;">
                Atenciosamente,<br>
                <strong>Equipe Preventech</strong>
                </p>
            </div>
            </body>
            </html>
            """;
    
        public static string MensagemSiteConcluida(Models.OrdemServico os) =>
            $"""
                Olá {os.Requisitante!.Nome},
                
                Sua Ordem de Serviço {os.Titulo} (ID: {os.Id}) foi concluída com sucesso pelo técnico {os.TecnicoResponsavel!.Nome}.
                {(string.IsNullOrWhiteSpace(os.DescricaoSolucao) ? "" : $"\nAções Realizadas:\n{os.DescricaoSolucao}\n")}
                Você pode visualizar seu email para mais informações.
                
                Se tiver qualquer dúvida, entre em contato com nossa equipe de suporte.
                
                Atenciosamente,
                Equipe Preventech
            """;
    }

    public static class Equipamento
    {
        public static string TituloManutencaoPendente(string nomeEquipamento) =>
            $"Manutenção Pendente - {nomeEquipamento}";
        
        public static string MensagemManutencaoPendente(string nomeEquipamento, DateTime dataVencimento) =>
            $"O equipamento {nomeEquipamento} possui manutenção pendente.\nData prevista: {dataVencimento:dd/MM/yyyy}\nPor favor, agende a manutenção o quanto antes.";

        public static string TituloManutencaoAtrasada(string nomeEquipamento) =>
            $"⚠️ Manutenção Atrasada - {nomeEquipamento}";
        
        public static string MensagemManutencaoAtrasada(string nomeEquipamento, int diasAtraso) =>
            $"ATENÇÃO: A manutenção do equipamento {nomeEquipamento} está atrasada há {diasAtraso} dias.\nProvidências imediatas são necessárias.";
    }

    public static class Sistema
    {
        public static string TituloAtualizacao => "Atualização do Sistema";
        
        public static string MensagemAtualizacao(string versao, string dataManutencao) =>
            $"O sistema será atualizado para a versão {versao}.\nManutenção programada para: {dataManutencao}\nDurante este período, o sistema ficará temporariamente indisponível.";

        public static string TituloManutencao => "Manutenção Programada";
        
        public static string MensagemManutencao(DateTime inicio, DateTime fim) =>
            $"Manutenção programada do sistema.\nInício: {inicio:dd/MM/yyyy HH:mm}\nFim previsto: {fim:dd/MM/yyyy HH:mm}\n\nPedimos desculpas pelo inconveniente.";
    }
}
