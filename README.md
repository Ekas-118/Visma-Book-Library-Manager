# Visma Lietuva Summer Internship 2021 .NET Developer Task
## Requirements:
- Command to add a new book. All the book data should be stored in a JSON file.
Book model should contain the following properties:
  - Name
  - Author
  - Category
  - Language
  - Publication date
  - ISBN
- Command to take a book from the library. The command should specify who is taking
the book and for what period the book is taken. Taking the book longer than two
months should not be allowed. Taking more than 3 books should not be allowed.
- Command to return a book.
  - If a return is late you could display a funny message :)
- Command to list all the books. Add the following parameters to filter the data:
  - Filter by author
  - Filter by category
  - Filter by language
  - Filter by ISBN
  - Filter by name
  - Filter taken or available books.
- Command to delete a book.
## Commands

- `manager [subcommand] <-h / --help / -?>`  
Displays help for input command

- `manager add <name> <author> <category> <language> <year> <isbn>`  
Adds a book to the library
- `manager take <isbn> <reader> <months> <days>`  
Lends a book to a reader
- `manager return <isbn>`  
Returns the book to the library
- `manager list [--byAuthor <author>] [--byCategory <category>] [--byLanguage <language>] [--byISBN <isbn>] [--byName <name>] [--byStatus <Taken/Available>]`  
Lists all the books in the library
- `manager delete <isbn>`  
Deletes a book from the library
