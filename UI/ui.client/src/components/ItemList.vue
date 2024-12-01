<template>
  <div class="main-block">

    <div v-for="(group, category) in groupedAndSortedItems" :key="category" class="category-group">
      <h2 class="category-name-view">{{ category }}</h2> <h2 class="category-total-view">${{ Number(group.total).toFixed(2) }}</h2>
      <div class="item-view" v-for="item in group.items" :key="item.id">
        <div class="item-name-view">{{ item.name }}</div>
        <div class="item-value-view">${{ Number(item.value).toFixed(2) }}</div>
        <div class="item-delete-button">
          <button @click="onDelete(item.id)" class="delete-button">
            <i class="fas fa-trash"></i>
          </button>
        </div>
      </div>
    </div>

    <div class="total-group">
      <h2 class="total-label-view">Total</h2> <h2 class="total-value-view">${{ Number(finalTotalSum).toFixed(2) }}</h2>
    </div>
    <div class="add-item-block">
      <AddItem @addItem="onAddItem"></AddItem>
    </div>
  </div>
</template>

<script>
  import "@fortawesome/fontawesome-free/css/all.css";
  import "@fortawesome/fontawesome-free/js/all.js";
  import { ref, computed, onMounted } from "vue";
  import AddItem from "./AddItem.vue";
  import axios from "axios";
  import { apiFetchItems, apiAddItem, apiDeleteItem } from "../api/api";

  export default {
    name: "ItemList",
    components: {
      AddItem,
    },
    setup() {
      // Reactive variables
      const items = ref([]);
      const newItem = ref({
        name: "",
        value: null,
        categoryId: "",
      });

      // Fetch all items from the Web API
      const fetchItems = async () => {
        try {
          items.value = await apiFetchItems();
        } catch (error) {
          console.error("Error fetching items:", error);
        }
      };

      // Handle new item addition from the AddItem component
      const onAddItem = async (newItem) => {
        var addedItem = await apiAddItem(newItem);
        items.value.push(addedItem);
      };

      // Delete an item by ID
      const onDelete = async (id) => {
        try {
          await apiDeleteItem(id);
          items.value = items.value.filter((item) => item.id !== id);
        } catch (error) {
          console.error("Error deleting item:", error);
        }
      };

      // Group items, calculate category sums, and sort by category name
      const groupedAndSortedItems = computed(() => {
        const grouped = items.value.reduce((acc, item) => {
          const category = item.categoryName || "Uncategorized";
          if (!acc[category]) {
            acc[category] = { items: [], total: 0 };
          }
          acc[category].items.push(item);
          acc[category].total += item.value;
          return acc;
        }, {});

        // Sort categories alphabetically
        const sortedCategories = Object.keys(grouped).sort();

        // Rebuild the grouped object in sorted order
        const sortedGrouped = sortedCategories.reduce((acc, category) => {
          acc[category] = grouped[category];
          acc[category].items.sort((a, b) => a.value - b.value);
          return acc;
        }, {});

        return sortedGrouped;
      });

      // Compute the final total sum of all items
      const finalTotalSum = computed(() =>
        items.value.reduce((sum, item) => sum + item.value, 0)
      );

      onMounted(() => {
        fetchItems();
      });

      return {
        items,
        newItem,
        fetchItems,
        onAddItem,
        onDelete,
        groupedAndSortedItems,
        finalTotalSum,
      };
    },
  };
</script>

<style>
  .main-block {
    margin: auto;
    width: max-content;
    /*padding: 5px;*/
    border: 1px solid black;
  }

  .item-view {
    margin-left: 20px;
  }

  .item-name-view {
    float: left;
    width: 200px;
  }

  .item-value-view {
    float: left;
    width: 70px;
  }

  .item-delete-button {
    float: left;
  }

  .category-name-view {
    float: left;
    width: 200px;
  }

  .category-total-view {
    float: left;
  }

  .category-group {
    padding: 5px;
  }

  .total-label-view {
    float: left;
    width: 200px;
  }

  .total-value-view {
    float: left;
    width: 50px;
  }

  .total-group {
    padding: 5px;
  }

  .delete-button {
    background: none;
    border: none;
    color: black;
    cursor: pointer;
    margin-left: 10px;
    font-size: 16px;
  }

  .delete-button i {
    margin-right: 5px;
  }

  .add-item-block {
    clear: left;
    margin: 0px;
    padding: 0px;
    border: 1px solid black;
  }
</style>
