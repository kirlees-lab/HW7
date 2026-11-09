namespace HW7
{
    class BookList
    {
        private List<string> books = new List<string>();

        public int Count
        {
            get { return books.Count; }
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= books.Count)
                {
                    throw new IndexOutOfRangeException("Нет книги с таким номером");
                }
                return books[index];
            }
            set
            {
                if (index < 0 || index >= books.Count)
                {
                    throw new IndexOutOfRangeException("Нет книги с таким номером");
                }
                books[index] = value;
            }
        }

        public void Add(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Название книги не может быть пустым");
            }
            books.Add(title);
        }

        public bool Remove(string title)
        {
            return books.Remove(title);
        }

        public bool Contains(string title)
        {
            return books.Contains(title);
        }

        public static BookList operator +(BookList list, string title)
        {
            list.Add(title);
            return list;
        }

        public static BookList operator -(BookList list, string title)
        {
            list.Remove(title);
            return list;
        }
    }

    class Fraction
    {
        private int numerator;
        private int denominator;

        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; }
        }

        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("Знаменатель не может быть равен нулю");
                }
                denominator = value;
            }
        }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        private static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public void Reduce()
        {
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            int gcd = Gcd(Math.Abs(numerator), denominator);
            numerator /= gcd;
            denominator /= gcd;
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.denominator + b.numerator * a.denominator,
                                           a.denominator * b.denominator);
            result.Reduce();
            return result;
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.denominator - b.numerator * a.denominator,
                                           a.denominator * b.denominator);
            result.Reduce();
            return result;
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
            result.Reduce();
            return result;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
            result.Reduce();
            return result;
        }

        public static bool operator ==(Fraction a, Fraction b)
        {
            return a.numerator * b.denominator == b.numerator * a.denominator;
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Fraction)
            {
                return this == (Fraction)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int n = numerator;
            int d = denominator;

            if (d < 0)
            {
                n = -n;
                d = -d;
            }

            int gcd = Gcd(Math.Abs(n), d);
            return (n / gcd).GetHashCode() ^ (d / gcd).GetHashCode();
        }

        public override string ToString()
        {
            return numerator + "/" + denominator;
        }
    }

    class Currency
    {
        private string name;
        private double rate;
        private double amount;

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название валюты не может быть пустым");
                }
                name = value;
            }
        }

        public double Rate
        {
            get { return rate; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Курс должен быть больше нуля");
                }
                rate = value;
            }
        }

        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Currency(string name, double rate, double amount)
        {
            Name = name;
            Rate = rate;
            Amount = amount;
        }

        public override string ToString()
        {
            return Math.Round(amount, 2) + " " + name;
        }
    }

    class USD : Currency
    {
        public USD(double amount) : base("USD", 41.5, amount)
        {
        }

        public USD(Currency other) : this(0)
        {
            Amount = other.Amount * other.Rate / Rate;
        }

        public static implicit operator EUR(USD usd)
        {
            return new EUR(usd);
        }

        public static explicit operator GBP(USD usd)
        {
            return new GBP(usd);
        }
    }

    class EUR : Currency
    {
        public EUR(double amount) : base("EUR", 45, amount)
        {
        }

        public EUR(Currency other) : this(0)
        {
            Amount = other.Amount * other.Rate / Rate;
        }

        public static implicit operator USD(EUR eur)
        {
            return new USD(eur);
        }

        public static explicit operator GBP(EUR eur)
        {
            return new GBP(eur);
        }
    }

    class GBP : Currency
    {
        public GBP(double amount) : base("GBP", 53, amount)
        {
        }

        public GBP(Currency other) : this(0)
        {
            Amount = other.Amount * other.Rate / Rate;
        }

        public static explicit operator USD(GBP gbp)
        {
            return new USD(gbp);
        }

        public static explicit operator EUR(GBP gbp)
        {
            return new EUR(gbp);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // N1
            BookList books = new BookList();
            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Добавить книгу");
                Console.WriteLine("2 - Удалить книгу");
                Console.WriteLine("3 - Проверить, есть ли книга в списке");
                Console.WriteLine("4 - Показать список");
                Console.WriteLine("5 - Заменить книгу по номеру");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());

                    if (choice == 1)
                    {
                        Console.Write("Название: ");
                        string title = Console.ReadLine();
                        books = books + title;
                        Console.WriteLine("Книга добавлена");
                    }
                    else if (choice == 2)
                    {
                        Console.Write("Название: ");
                        string title = Console.ReadLine();

                        if (books.Contains(title))
                        {
                            books = books - title;
                            Console.WriteLine("Книга удалена");
                        }
                        else
                        {
                            Console.WriteLine("Такой книги нет в списке");
                        }
                    }
                    else if (choice == 3)
                    {
                        Console.Write("Название: ");
                        string title = Console.ReadLine();

                        if (books.Contains(title))
                        {
                            Console.WriteLine("Книга есть в списке");
                        }
                        else
                        {
                            Console.WriteLine("Книги нет в списке");
                        }
                    }
                    else if (choice == 4)
                    {
                        Console.WriteLine("Книг в списке: " + books.Count);
                        for (int i = 0; i < books.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + books[i]);
                        }
                    }
                    else if (choice == 5)
                    {
                        Console.Write("Номер книги: ");
                        int number = int.Parse(Console.ReadLine());
                        Console.Write("Новое название: ");
                        books[number - 1] = Console.ReadLine();
                        Console.WriteLine("Книга заменена");
                    }
                    else if (choice == 0)
                    {
                        running = false;
                    }
                    else
                    {
                        Console.WriteLine("Нет такого пункта меню");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }

            // N2
            Console.WriteLine();
            Fraction f1 = new Fraction(1, 2);
            Fraction f2 = new Fraction(3, 4);
            Fraction f3 = new Fraction(2, 4);

            Console.WriteLine(f1 + " + " + f2 + " = " + (f1 + f2));
            Console.WriteLine(f1 + " - " + f2 + " = " + (f1 - f2));
            Console.WriteLine(f1 + " * " + f2 + " = " + (f1 * f2));
            Console.WriteLine(f1 + " / " + f2 + " = " + (f1 / f2));

            Console.WriteLine(f1 + " == " + f3 + ": " + (f1 == f3));
            Console.WriteLine(f1 + " != " + f2 + ": " + (f1 != f2));

            f3.Reduce();
            Console.WriteLine("После сокращения: " + f3);

            try
            {
                Fraction wrong = new Fraction(1, 0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            // N3
            Console.WriteLine();
            USD usd = new USD(100);
            Console.WriteLine("Исходная сумма: " + usd);

            EUR eur = usd;
            Console.WriteLine("Неявно USD -> EUR: " + eur);

            USD usdBack = eur;
            Console.WriteLine("Неявно EUR -> USD: " + usdBack);

            GBP gbp = (GBP)usd;
            Console.WriteLine("Явно USD -> GBP: " + gbp);

            EUR eurFromGbp = (EUR)gbp;
            Console.WriteLine("Явно GBP -> EUR: " + eurFromGbp);
        }
    }
}
