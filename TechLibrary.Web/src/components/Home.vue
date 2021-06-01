<template>
    <div class="home">
        <b-table striped hover :items="GetWithPagination" :fields="fields" responsive="sm" id="table" :current-page="currentPage">
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>
        <b-pagination v-model="currentPage" :total-rows="pagination"></b-pagination>

        <p class="mt-3">Current Page: {{ currentPage }}</p>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        props: {},
        data: () => ({
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

            ],
            items: [],
            currentPage: 1,
            pagination: 0
        }),
        methods: {
            GetWithPagination(ctx, callback) {
                axios.get('https://localhost:5001/books/pagination/' + this.currentPage)
                    .then(response => {
                        callback(response.data);
                    });
            }
        },
        watch: {
            initialize: {
                immediate: true,
                handler() {
                    axios.get('https://localhost:5001/books/pagination')
                        .then(response => {
                            this.pagination = response.data;
                        });
                }
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>