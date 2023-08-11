# EntityFramework.Exceptions

## Введение
Во время работы приложения или сервиса могут возникнуть ошибки связанные с работой с базой данных, например при добавлении, обновлении или удалении данных.

Для большенства случаев при возникновении ошибки будет сгенерирована стандартная ошибка типа DbException или DbUpdateException.

Для понимания причины возникновения ошибки придется обратиться к содержимому InnerException, а если нужна конкретная реакция на конкретную ошибку, то потребуется реализовать в коде парсинг InnerException и добавить нужную логику в блок try/catch.

Естественно подобные действия несут много проблем и много лишнего кода, что неизбежно ведет к еще большим ошибкам.

Для того, чтобы получить описание конкретной причины возникновения ошибки можно  воспользоваться библиотекой EntityFramework.Exceptions.

Она предоставляет возможность вернуть сообщение об ошибке в удобном для обработки виде.
## Установка
Для работы с этим пакетом, его нужно всего лишь установить в проект и добавить в расширяющий метод IServiceCollection для регистрации текущего DbContext.

Для примера возьмем пакет для работы с PostgreSQL. (Аналогично можно установить пакеты для SQLServer, SQLite, Oracle и MySql)

`dotnet add package EntityFrameworkCore.Exceptions.PostgreSQL` 

После установки пакета, в метод конфигурации построителя контекста для заданной БД добавляем UseExceptionProcessor().

```cs
builder.Services
    .AddDbContext<DreamComeTrueContext>(
        optionsBuilder =>
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb"))
    .UseExceptionProcessor()
);
```
или в OnConfiguring()

```cs
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ///some middleware
        optionsBuilder.UseExceptionProcessor();
        ///some middleware
    }
   ```


В результате при возникновении ошибки она будет перехвачена и возвращена.

## Пример
Например при попытке создать сущность в БД без описания обязательного поля, ошибки будут разниться:

Без пакета
 ```
Exception Type =     Microsoft.EntityFrameworkCore.DbUpdateException
Exception Message =  An error occurred while saving the entity changes. See the inner exception for details.
```
 С пакетом

```
Exception Type =     EntityFramework.Exceptions.Common.ReferenceConstraintException
Exception Message =  Reference constraint violation.
```
Для обработки ошибки воспользуемся блоком _try-catch_.

   ```cs
        try
        {
            _dataRepository.AddEntity(entity);
            await _dataRepository.SaveChanges();
        }
        catch (ReferenceConstraintException ex)
        {
            if (ex.InnerException != null)
            {
                var message = new DataExceptions(ex.Message);
                throw new MyException(message, 400);
            }
        }
   ```
   
## Выводы

EntityFramework.Exceptions значительно упрощает обработку исключений связанных с
ограничениями по уникальному значению, слишком длинным значением или отсутствием обязательного значения.

Все исключения наследуются от DbUpdateException, что обеспечивает обратную совместимость:

* UniqueConstraintException
* CannotInsertNullException
* MaxLengthExceededException
* NumericOverflowException
* ReferenceConstraintException.

Пакет имеет поддержку SQLServer, PostgreSQL, SQLite, Oracle и MySql.


