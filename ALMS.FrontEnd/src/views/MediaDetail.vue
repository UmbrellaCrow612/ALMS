<template>
    <div class="min-h-screen bg-background">
      <main class="container mx-auto px-4 py-8">
        <div class="grid gap-6 lg:grid-cols-3">
          <!-- Left Column - Media Photo and Borrow History -->
          <div class="space-y-6">
            <!-- Media Image Card -->
            <Card>
                <CardContent className="p-6">
        <div className="aspect-square relative bg-muted rounded-lg overflow-hidden">
          <img 
            src="https://picsum.photos/200/300" 
            alt="Sample image" 
            className="object-cover w-full h-full"
          />
        </div>
      </CardContent>
            </Card>
  
            <!-- Borrow History Card -->
            <Card>
              <CardContent class="p-6">
                <h2 class="text-lg font-semibold mb-4 flex items-center text-theme-primary">
                  <Clock class="mr-2 h-5 w-5" />
                  Borrow History
                </h2>
                <div class="space-y-4">
                  <div
                    v-for="(history, index) in mediaDetails.borrowHistory"
                    :key="index"
                    class="flex items-center justify-between text-sm"
                  >
                    <div class="flex items-center text-muted-foreground">
                      <Calendar class="mr-2 h-4 w-4 text-theme-muted" />
                      <span>{{ history.date }}</span>
                    </div>
                    <span class="text-green-600">{{ history.status }}</span>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
  
          <!-- Right Column - Description and Similar Items -->
          <div class="lg:col-span-2 space-y-6">
            <!-- Media Details Card -->
            <Card>
              <CardContent class="p-6">
                <div class="space-y-4">
                  <div class="flex items-center justify-between">
                    <h1 class="text-2xl font-bold text-theme-primary">{{ mediaDetails.title }}</h1>
                    <span class="text-sm text-theme-muted">{{ mediaDetails.type }}</span>
                  </div>
                  <div class="flex items-center text-theme-muted">
                    <User class="mr-2 h-4 w-4" />
                    <span>{{ mediaDetails.author }}</span>
                  </div>
                  <Separator />
                  <div class="space-y-2">
                    <h2 class="font-semibold text-theme-primary">Description</h2>
                    <p class="text-theme-muted">{{ mediaDetails.description }}</p>
                  </div>
                  <div class="grid grid-cols-2 gap-4 text-sm">
                    <div>
                      <span class="font-medium">Genre:</span> {{ mediaDetails.genre }}
                    </div>
                    <div>
                      <span class="font-medium">Published:</span> {{ mediaDetails.publishedYear }}
                    </div>
                    <div>
                      <span class="font-medium">ISBN:</span> {{ mediaDetails.isbn }}
                    </div>
                    <div>
                      <span class="font-medium">Available:</span> {{ mediaDetails.availableCopies }} of {{ mediaDetails.totalCopies }}
                    </div>
                  </div>
                </div>
              </CardContent>
            </Card>
  
            <!-- Similar Items Card -->
            <Card>
              <CardContent class="p-6">
                <h2 class="text-lg font-semibold mb-4 text-theme-primary">Similar Items</h2>
                <ScrollArea class="h-[200px] pr-4">
                  <div class="space-y-4">
                    <div
                      v-for="item in mediaDetails.similarItems"
                      :key="item.id"
                      class="flex items-center justify-between p-3 rounded-lg hover:bg-muted transition-colors"
                    >
                      <div class="flex items-center">
                        <BookOpen class="h-4 w-4 mr-3 text-theme-muted" />
                        <div>
                          <div class="font-medium text-theme-primary">{{ item.title }}</div>
                          <div class="text-sm text-theme-muted">{{ item.author }}</div>
                        </div>
                      </div>
                      <Button variant="ghost" size="icon">
                        <ChevronRight class="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
  
            <!-- Reserve and Borrow Buttons -->
            <div class="flex justify-end gap-4">
              <Button @click="reserveItem" variant="outline" class="w-32">
                <BookMarked class="mr-2 h-4 w-4" />
                Reserve
              </Button>
              <Button @click="borrowItem" class="w-32 bg-theme-primary text-white">
                <BookOpen class="mr-2 h-4 w-4" />
                Borrow
              </Button>
            </div>
          </div>
        </div>
      </main>
    </div>
  </template>
  
  <script>
  import { ref } from 'vue'
  import { Clock, Calendar, User, BookOpen, ChevronRight, BookMarked } from 'lucide-vue-next'
  
  export default {
    name: 'MediaDetail',
    components: { Clock, Calendar, User, BookOpen, ChevronRight, BookMarked },
    setup() {
      const mediaDetails = ref({
        id: 1,
        title: 'To Kill a Mockingbird',
        author: 'Harper Lee',
        type: 'Book',
        genre: 'Fiction',
        description:
          'Set in the American South, the novel tells the story of a lawyer, Atticus Finch, and his young children as they navigate issues of racial injustice in their small town. A compelling narrative about moral growth and the complexities of human nature, told through the eyes of young Scout Finch.',
        publishedYear: 1960,
        isbn: '978-0446310789',
        availableCopies: 3,
        totalCopies: 5,
        borrowHistory: [
          { date: '2024-01-15', returnDate: '2024-02-15', status: 'Returned' },
          { date: '2023-11-20', returnDate: '2023-12-20', status: 'Returned' },
        ],
        similarItems: [
          { id: 2, title: 'The Great Gatsby', author: 'F. Scott Fitzgerald', type: 'Book' },
          { id: 3, title: 'The Catcher in the Rye', author: 'J.D. Salinger', type: 'Book' },
          { id: 4, title: 'Lord of the Flies', author: 'William Golding', type: 'Book' },
          { id: 5, title: '1984', author: 'George Orwell', type: 'Book' },
        ],
      })
  
      // Button handlers
      const reserveItem = () => {
        alert('Reserve function clicked!')
      }
  
      const borrowItem = () => {
        alert('Borrow function clicked!')
      }
  
      return {
        mediaDetails,
        reserveItem,
        borrowItem,
      }
    },
  }
  </script>
  
  <style scoped>
  .container {
    max-width: 1200px;
  }
  
  /* Custom theme colors */
  .text-theme-primary {
    color: #2c3e50; /* Example dark primary color */
  }
  
  .text-theme-muted {
    color: #7f8c8d; /* Subtle gray for muted text */
  }
  
  .bg-theme-primary {
    background-color: #2980b9; /* Button primary color */
  }
  
  .bg-muted {
    background-color: #f5f5f5;
  }
  </style>
  