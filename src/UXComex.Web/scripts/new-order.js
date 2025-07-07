const items = [];
let clientId = "";
let totalValue = 0;

$(() => {
  const getQueryParameter = (param) =>
    new URLSearchParams(document.location.search.substring(1)).get(param);

  const paramClientId = getQueryParameter("clientId");

  if (paramClientId) {
    clientId = paramClientId;
    $("#clientInput").prop("disabled", true);
    $.get(`https://localhost:7150/api/clients/${clientId}`, (data) => {
      $("#clientInput").empty();
      const option = `<option value="${data.id}">${data.name}</option>`;
      $("#clientInput").append(option);
    });
  } else {
    $("#clientInput").empty();
    $.get(`https://localhost:7150/api/clients`, (data) => {
      $("#clientInput").append(
        '<option value="" selected>Selecione um cliente</option>'
      );
      data.items.forEach((client) => {
        const option = `<option value="${client.id}">${client.name}</option>`;
        $("#clientInput").append(option);
      });
    });
  }
});

$(document).on("click", "#searchProductsButton", (element) => {
  element.preventDefault();
  const search = $("#productInput").val();
  $.get(`https://localhost:7150/api/products?&search=${search}`, (data) => {
    const tbody = $("#productSearchTBody");
    tbody.empty();
    data.items.forEach((product) => {
      const row = `<tr class="row">
            <td class="col">${product.name}</td>
            <td class="col">${product.description}</td>
            <td class="col">${product.price.toFixed(2).replace(".", ",")}</td>
            <td class="col-1">
                <div>
                <input
                    type="number"
                    class="form-control"
                    id="quantityInput-${product.id}"
                    value="1"
                />
                </div>
            </td>
            <td class="col-1">
                <div
                class="btn-group"
                role="group"
                aria-label="Basic example"
                >
                <button type="button" id="addNewProductButton" value="${
                  product.id
                }" class="btn btn-primary">
                    <i class="bi bi-plus"></i>
                </button>
                </div>
            </td>
        </tr>`;
      tbody.append(row);
    });
  }).fail(() => {
    console.error("Erro ao pesquisar produto!");
  });
});

$(document).on("click", "#addNewProductButton", (element) => {
  element.preventDefault();
  const productId = $(element.currentTarget).val();
  const quantity = $(`#quantityInput-${productId}`).val();

  $.get(`https://localhost:7150/api/products/${productId}`, (data) => {
    const tbody = $("#productInBasketTBody");
    const row = `<tr class="row" id="row-${data.id}">
            <td class="col">${data.name}</td>
            <td class="col">${(quantity * data.price)
              .toFixed(2)
              .replace(".", ",")}</td>
            <td class="col">
                <div>
                <input
                    type="number"
                    class="form-control"
                    value="${quantity}"
                    readonly
                />
                </div>
            </td>
            <td class="col">
                <div
                class="btn-group"
                role="group"
                aria-label="Basic example"
                >
                <button type="button" id="removeProductButton" value="${
                  data.id
                }" class="btn btn-danger">
                    <i class="bi bi-trash"></i>
                </button>
                </div>
            </td>
        </tr>`;

    items.push({
      productId,
      quantity,
      price: data.price,
    });

    tbody.append(row);
    $("#productInput").val("");

    const currentValue = totalValue;
    const newValue = currentValue + data.price * quantity;
    totalValue = newValue;
    $("#totalValueLabel").text(newValue.toFixed(2).replace(".", ","));
  });
});

$(document).on("click", "#removeProductButton", (element) => {
  element.preventDefault();
  const productId = $(element.currentTarget).val();
  $(`#row-${productId}`).remove();

  var product = items.find((x) => x.productId === productId);
  const currentValue = totalValue;
  const newValue = currentValue - product.price * product.quantity;
  totalValue = newValue;
  items.splice(items.indexOf(product));
  $("#totalValueLabel").text(newValue.toFixed(2).replace(".", ","));
});

$(document).on("click", "#newOrderButton", (element) => {
  element.preventDefault();

  $.ajax({
    url: "https://localhost:7150/api/orders",
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify({
      clientId,
      orderItems: items.map((x) => {
        return {
          productId: x.productId,
          quantity: parseInt(x.quantity),
        };
      }),
    }),
    success: () => {
      $(location).attr("href", "../pages/orders.html");
      alert("Pedido adicionado com sucesso!");
    },
    error: () => {
      alert("Erro ao concluir pedido!");
    },
  });
});

$(document).on("click", "#clientInput", (element) => {
  element.preventDefault();
  clientId = $(element.currentTarget).val();
});
