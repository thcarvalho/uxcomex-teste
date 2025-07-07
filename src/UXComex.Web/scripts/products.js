// import $ from "jquery"

getData();

$(() => {
  $("#addProductButton").on("click", (event) => {
    event.preventDefault();
    const name = $("#nameInput").val();
    const description = $("#descriptionInput").val();
    const price = parseFloat($("#priceInput").val());
    const quantityInStock = parseInt($("#quantityInStockInput").val());

    $.ajax({
      url: "https://localhost:7150/api/products",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify({ name, description, price, quantityInStock }),
      success: () => {
        $("#addProductModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao adicionar produto!");
      },
    });
  });
});

$(document).on("click", "#editProductModalButton", element => {
    const productId = $(element.currentTarget).val();
  $("#editProductButton").data("productId", productId);

  $.get(`https://localhost:7150/api/products/${productId}`, (data) => {
    $("#productIdEditInput").val(productId);
    $("#nameEditInput").val(data.name);
    $("#descriptionEditInput").val(data.description);
    $("#priceEditInput").val(data.price);
    $("#quantityInStockEditInput").val(data.quantityInStock);

    $("#editProductModal").modal("show");
  });
});

$(document).on("click", "#deleteProductModalButton", element => {
    const productId = $(element.currentTarget).val();
  $("#deleteProductButton").data("productId", productId);
  $("#deleteProductModal").modal("show");
});

$(() => {
  $("#editProductButton").on("click", (event) => {
    event.preventDefault();
    const id = $("#productIdEditInput").val();
    const name = $("#nameEditInput").val();
    const description = $("#descriptionEditInput").val();
    const price = $("#priceEditInput").val();
    const quantityInStock = $("#quantityInStockEditInput").val();

    $.ajax({
      url: `https://localhost:7150/api/products/${id}`,
      type: "PUT",
      contentType: "application/json",
      data: JSON.stringify({ name, description, price, quantityInStock }),
      success: () => {
        $("#editProductModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao editar produto!");
      },
    });
  });
});

$(() => {
  $("#deleteProductButton").on("click", (event) => {
    event.preventDefault();
    const id = $("#deleteProductButton").data("productId");
    $.ajax({
      url: `https://localhost:7150/api/products/${id}`,
      type: "DELETE",
      contentType: "application/json",
      success: () => {
        $("#deleteProductModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao deletar produto!");
      },
    });
  });
});

$(() => {
  $("#searchButton").on("click", (event) => {
    event.preventDefault();
    const filter = $("#filterSelect").val();
    const search = $("#searchInput").val();
    getData(search, filter);
  });
});

function getData(search = "", field = 0, page = 1, itemsPerPage = 10) {
  let url = `https://localhost:7150/api/products?pageNumber=${page}&itemsPerPage=${itemsPerPage}`;
  if (search !== undefined) {
    url += `&search=${search}&field=${field}`;
  }
  $.get(url, (data) => {
    const tbody = $("table tbody");
    tbody.empty();
    data.items.forEach((product) => {
      const row = `<tr>
            <td hidden class="productId">${product.id}</td>
            <td>${product.name}</td>
            <td>${product.description}</td>
            <td>R$ ${product.price.toFixed(2).replace(".", ",")}</td>
            <td>${product.quantityInStock}</td>
            <td>
            <div class="btn-group" role="group">
                <button id="editProductModalButton" value="${product.id}" type="button" class="btn btn-outline-primary">
                <i class="bi bi-pencil"></i>
                </button>
                <button id="deleteProductModalButton" value="${product.id}"  type="button" class="btn btn-outline-primary">
                <i class="bi bi-trash"></i>
                </button>
            </div>
            </td>
        </tr>`;
      tbody.append(row);
    });

    const pagination = $("#paginationNav");
    pagination.empty();

    const totalPages = data.totalPages;
    const currentPage = data.page;

    const previous = data.page > 1 
      ? `<li class="page-item"><a class="page-link" href="#" onclick="getData('${search}', '${field}', ${data.page - 1}, ${itemsPerPage})">Anterior</a></li>`
      : `<li class="page-item disabled"><a class="page-link">Anterior</a></li>`

    const next = data.page === totalPages
      ? `<li class="page-item disabled"><a class="page-link">Próxima</a></li>`
      : `<li class="page-item"><a class="page-link" href="#" onclick="getData('${search}', '${field}', ${data.page + 1}, ${itemsPerPage})">Próxima</a></li>`

    let pages = ""

    for (let index = 1; index <= totalPages; index++) {
      pages += 
       `<li class="page-item ${index === currentPage ? 'active' : ''}">
          <a class="page-link" href="#" onclick="getData('${search}', '${field}', ${index}, ${itemsPerPage})">${index}</a>
        </li>`            
    }  
    const pagRows = 
    `
      <ul class="pagination">
        ${previous}
        ${pages}
        ${next}
      </ul>
    `
    pagination.append(pagRows);
  }).fail((error) => {
    alert("Erro ao pesquisar produtos!");
  });
}
