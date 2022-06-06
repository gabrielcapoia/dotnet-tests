using NerdStore.Sales.Application.Queries.ViewModels;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IOrderRepository orderRepository;

        public PedidoQueries(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId)
        {
            var pedido = await orderRepository.GetDraftedOrderByCustomerId(clienteId);
            if (pedido == null) return null;

            var carrinho = new CarrinhoViewModel
            {
                ClienteId = pedido.CustomerId,
                ValorTotal = pedido.Amount,
                PedidoId = pedido.Id,
                ValorDesconto = pedido.DiscountAmount,
                SubTotal = pedido.DiscountAmount + pedido.Amount
            };

            if (pedido.Voucher != null)
            {
                carrinho.VoucherCodigo = pedido.Voucher.Code;
            }

            foreach (var item in pedido.OrderItems)
            {
                carrinho.Items.Add(new CarrinhoItemViewModel
                {
                    ProdutoId = item.ProductId,
                    ProdutoNome = item.ProductName,
                    Quantidade = item.Quantity,
                    ValorUnitario = item.UnitValue,
                    ValorTotal = item.UnitValue * item.Quantity
                });
            }

            return carrinho;
        }

        public async Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId)
        {
            var pedidos = await orderRepository.GetListByCustomerId(clienteId);

            pedidos = pedidos.Where(p => p.OrderStatus == OrderStatus.Paid|| p.OrderStatus == OrderStatus.Canceled)
                .OrderByDescending(p => p.Code);

            if (!pedidos.Any()) return null;

            var pedidosView = new List<PedidoViewModel>();

            foreach (var pedido in pedidos)
            {
                pedidosView.Add(new PedidoViewModel
                {
                    Id = pedido.Id,
                    ValorTotal = pedido.Amount,
                    PedidoStatus = (int)pedido.OrderStatus,
                    Codigo = pedido.Code,
                    DataCadastro = pedido.CreatedAt
                });
            }

            return pedidosView;
        }
    }
}
