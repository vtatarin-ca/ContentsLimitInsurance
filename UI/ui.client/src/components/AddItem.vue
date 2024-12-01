<template>
  <div>
    <form @submit.prevent="handleAdd">
      <span>
        <input class="input-name" v-model="nameInput" placeholder="Item Name" type="text" required />
      </span>
      <span>
        <input class="input-value" v-model="valueInput" @blur="formatValue" type="text" required />
      </span>
      <span>
        <select class="select-category" v-model="categoryIdSelect" required>
          <option v-for="(category, index) in categories" :key="category.id" :value="category.id" v-bind:selected="index === 0 ? 'selected' : ''">
            {{ category.name }}
          </option>
        </select>
      </span>
      <button class="add-button" type="submit">Add</button>
    </form>
  </div>
</template>

<script>
  import { ref, computed, onMounted } from "vue";
  import { apiFetchCategories, apiAddItem } from "../api/api";

  export default {
    name: "AddItem",
    emits: ["addItem"],
    setup(props, { emit }) {

      let valueInput = ref("0");
      let nameInput = ref("");
      let categories = ref([]);
      let categoryIdSelect = ref("");

      const formatValue = () => {
        const numericValue = parseFloat(valueInput.value.replace(/[^0-9.]/g, "")) || 0;
        valueInput.value = `$${Number(numericValue).toFixed(2)}`;
      };

      // Fetch categories from API
      const fetchCategories = async () => {
        try {
          categories.value = await apiFetchCategories();

          // Preselect the first category
          categoryIdSelect.value = categories.value[0].id;
        } catch (error) {
          console.error("Error fetching categories:", error);
        }
      };

      // Fetch categories on mount
      onMounted(fetchCategories);

      const handleAdd = async () => {
        try {
          const addItemModel = {
            name: nameInput.value,
            value: valueInput.value.replaceAll("$", ""),
            categoryId: categoryIdSelect.value,
          };
          console.log('handleAdd = ' + JSON.stringify(addItemModel));
          emit("addItem", addItemModel);

          valueInput.value = "0";
          nameInput.value = "";
        } catch (error) {
          console.error("Error adding item:", error);
        }
      };

      return {
        nameInput,
        valueInput,
        categoryIdSelect,
        categories,
        formatValue,
        handleAdd,
      };
    },
  };
</script>


<style>

  .input-name {
    width: 120px;
    height: 22px;
    border: solid 1px black;
    font-weight: normal;
    box-sizing: border-box;
  }

  .input-value {
    width: 70px;
    height: 22px;
    font-weight: normal;
    border: solid 1px black;
    box-sizing: border-box;
  }

  .select-category {
    width: 120px;
    height: 22px;
    font-weight: normal;
    border: solid 1px black;
    box-sizing: border-box;
  }

  .add-button {
    height: 22px;
    box-sizing: border-box;
  }
</style>
