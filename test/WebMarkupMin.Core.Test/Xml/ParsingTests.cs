using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Xml
{
	public class ParsingTests
	{
		[Fact]
		public void ParsingNonTrivialMarkupIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<?xml	   version=\'1.0\'   encoding=\"UTF-8\"		  \n  \t  ?>\n" +
				"<?xml-stylesheet  	 type=\"text/xsl\"    href=\"http://feeds.example.com/feed-rss.xslt\"  \n \t ?>\n" +
				"<rss	 version  =   \"2.0\"  \n>\n" +
				"	<channel  >\n" +
				"		<!-- Channel properties -->\n" +
				"		<title	>RSS Title</title  >\n" +
				"		<description>This is an example of an RSS feed</description>\n" +
				"		<link>http://www.example.com/rss</link>\n" +
				"		<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>\n" +
				"		<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"		<ttl  \n  >1800</ttl  \n>\n" +
				"		<!-- /Channel properties -->\n" +
				"		<!-- Item list -->\n" +
				"		<item >\n" +
				"			<title>Example entry</title>\n" +
				"			<description>\n" +
				"			<![CDATA[\n" +
				"			<p>Here is some text containing an description.</p>\n" +
				"			]]>\n" +
				"			</description>\n" +
				"			<link>http://www.example.com/2012/09/01/my-article</link>\n" +
				"			<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>\n" +
				"			<pubDate  \t\t >Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"		</item>\n" +
				"		<!-- /Item list -->\n" +
				"	</channel>\n" +
				"</rss>"
				;
			const string targetOutput1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<?xml-stylesheet type=\"text/xsl\" href=\"http://feeds.example.com/feed-rss.xslt\"?>\n" +
				"<rss version=\"2.0\">\n" +
				"	<channel>\n" +
				"		<!-- Channel properties -->\n" +
				"		<title>RSS Title</title>\n" +
				"		<description>This is an example of an RSS feed</description>\n" +
				"		<link>http://www.example.com/rss</link>\n" +
				"		<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>\n" +
				"		<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"		<ttl>1800</ttl>\n" +
				"		<!-- /Channel properties -->\n" +
				"		<!-- Item list -->\n" +
				"		<item>\n" +
				"			<title>Example entry</title>\n" +
				"			<description>\n" +
				"			<![CDATA[\n" +
				"			<p>Here is some text containing an description.</p>\n" +
				"			]]>\n" +
				"			</description>\n" +
				"			<link>http://www.example.com/2012/09/01/my-article</link>\n" +
				"			<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>\n" +
				"			<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"		</item>\n" +
				"		<!-- /Item list -->\n" +
				"	</channel>\n" +
				"</rss>"
				;

			const string input2 = "<?xml version=\"1.0\" standalone=\"no\" ?>\n" +
				"<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"\n" +
				"    \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\n" +
				"<svg width=\"100%\" height=\"100%\" version=\"1.1\"\n" +
				"     xmlns=\"http://www.w3.org/2000/svg\"\n" +
				"     xmlns:xlink=\"http://www.w3.org/1999/xlink\">\n" +
				"  <rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" \n" +
				"        style=\"fill: none; stroke: black; stroke-width: 5px;\" />\n\n" +
				"  <rect id=\"redRect\" x=\"100\" y=\"100\" width=\"50\" height=\"50\"\n" +
				"        fill=\"red\" stroke=\"blue\" />\n\n" +
				"</svg>"
				;
			const string targetOutput2 = "<?xml version=\"1.0\" standalone=\"no\"?>\n" +
				"<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" " +
				"\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\n" +
				"<svg width=\"100%\" height=\"100%\" version=\"1.1\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\">\n" +
				"  <rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" " +
				"style=\"fill: none; stroke: black; stroke-width: 5px;\"/>\n\n" +
				"  <rect id=\"redRect\" x=\"100\" y=\"100\" width=\"50\" height=\"50\" " +
				"fill=\"red\" stroke=\"blue\"/>\n\n" +
				"</svg>"
				;

			const string input3 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>\n" +
				"<КоммерческаяИнформация ВерсияСхемы=\"2.05\"\n" +
				"                ФорматДаты=\"ДФ=yyyy-MM-dd; ДЛФ=DT\"\n" +
				"                ФорматВремени=\"ДФ=ЧЧ:мм:сс; ДЛФ=T\"\n" +
				"                РазделительДатаВремя=\"T\"\n" +
				"                ФорматСуммы=\"ЧЦ=18; ЧДЦ=2; ЧРД=.\"\n" +
				"                ФорматКоличества=\"ЧЦ=18; ЧДЦ=2; ЧРД=.\">\n" +
				"  <Документ>\n" +
				"    <Ид>778BB97A-B2C7-4D6E-9379-5F26F69A3DDC</Ид>\n" +
				"    <Номер>5</Номер>\n" +
				"    <Дата>2013-04-10</Дата>\n" +
				"    <ХозОперация>Заказ товара</ХозОперация>\n" +
				"    <Роль>Продавец</Роль>\n" +
				"    <Валюта>руб</Валюта>\n" +
				"    <Курс>1</Курс>\n" +
				"    <Сумма>33100.00</Сумма>\n" +
				"    <Контрагенты>\n" +
				"      <Контрагент>\n" +
				"        <Ид>A106C7D2-1A21-46A0-8C74-8D7B7D8575AD</Ид>\n" +
				"        <Наименование>ООО \"Рога и копыта\"</Наименование>\n" +
				"        <ОфициальноеНаименование></ОфициальноеНаименование>\n" +
				"        <ЮридическийАдрес>\n" +
				"          <Представление></Представление>\n" +
				"        </ЮридическийАдрес>\n" +
				"        <Роль>Покупатель</Роль>\n" +
				"      </Контрагент>\n" +
				"    </Контрагенты>\n" +
				"    <Время>12:14:19</Время>\n" +
				"    <Комментарий></Комментарий>\n" +
				"    <Товары>\n" +
				"      <Товар>\n" +
				"        <Ид>33</Ид>\n" +
				"        <ИдКаталога></ИдКаталога>\n" +
				"        <Наименование>Диван</Наименование>\n" +
				"        <БазоваяЕдиница Код=\"796\" НаименованиеПолное=\"Штука\"\n" +
				"                   МеждународноеСокращение=\"PCE\">шт\n" +
				"        </БазоваяЕдиница>\n" +
				"        <ЦенаЗаЕдиницу>10000.00</ЦенаЗаЕдиницу>\n" +
				"        <Количество>1.00</Количество>\n" +
				"        <Сумма>10000</Сумма>\n" +
				"        <ЗначенияРеквизитов>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ВидНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ТипНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"        </ЗначенияРеквизитов>\n" +
				"      </Товар>\n" +
				"      <Товар>\n" +
				"        <Ид>34</Ид>\n" +
				"        <ИдКаталога></ИдКаталога>\n" +
				"        <Наименование>Кресло</Наименование>\n" +
				"        <БазоваяЕдиница Код=\"796\" НаименованиеПолное=\"Штука\"\n" +
				"                   МеждународноеСокращение=\"PCE\">шт\n" +
				"        </БазоваяЕдиница>\n" +
				"        <Скидки>\n" +
				"          <Скидка>\n" +
				"            <Наименование>Скидка на товар</Наименование>\n" +
				"            <Сумма>900.00</Сумма>\n" +
				"            <УчтеноВСумме>true</УчтеноВСумме>\n" +
				"          </Скидка>\n" +
				"        </Скидки>\n" +
				"        <ЦенаЗаЕдиницу>5100.00</ЦенаЗаЕдиницу>\n" +
				"        <Количество>1.00</Количество>\n" +
				"        <Сумма>5100</Сумма>\n" +
				"        <ЗначенияРеквизитов>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ВидНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ТипНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"        </ЗначенияРеквизитов>\n" +
				"      </Товар>\n" +
				"      <Товар>\n" +
				"        <Ид>348</Ид>\n" +
				"        <ИдКаталога></ИдКаталога>\n" +
				"        <Наименование>12 стульев</Наименование>\n" +
				"        <БазоваяЕдиница Код=\"796\" НаименованиеПолное=\"Штука\"\n" +
				"                   МеждународноеСокращение=\"PCE\">шт\n" +
				"        </БазоваяЕдиница>\n" +
				"        <ЦенаЗаЕдиницу>7000.00</ЦенаЗаЕдиницу>\n" +
				"        <Количество>1.00</Количество>\n" +
				"        <Сумма>7000</Сумма>\n" +
				"        <ЗначенияРеквизитов>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ВидНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ТипНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"        </ЗначенияРеквизитов>\n" +
				"      </Товар>\n" +
				"    </Товары>\n" +
				"  </Документ>\n" +
				"</КоммерческаяИнформация>"
				;
			const string targetOutput3 = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>\n" +
				"<КоммерческаяИнформация ВерсияСхемы=\"2.05\" " +
				"ФорматДаты=\"ДФ=yyyy-MM-dd; ДЛФ=DT\" " +
				"ФорматВремени=\"ДФ=ЧЧ:мм:сс; ДЛФ=T\" " +
				"РазделительДатаВремя=\"T\" " +
				"ФорматСуммы=\"ЧЦ=18; ЧДЦ=2; ЧРД=.\" " +
				"ФорматКоличества=\"ЧЦ=18; ЧДЦ=2; ЧРД=.\">\n" +
				"  <Документ>\n" +
				"    <Ид>778BB97A-B2C7-4D6E-9379-5F26F69A3DDC</Ид>\n" +
				"    <Номер>5</Номер>\n" +
				"    <Дата>2013-04-10</Дата>\n" +
				"    <ХозОперация>Заказ товара</ХозОперация>\n" +
				"    <Роль>Продавец</Роль>\n" +
				"    <Валюта>руб</Валюта>\n" +
				"    <Курс>1</Курс>\n" +
				"    <Сумма>33100.00</Сумма>\n" +
				"    <Контрагенты>\n" +
				"      <Контрагент>\n" +
				"        <Ид>A106C7D2-1A21-46A0-8C74-8D7B7D8575AD</Ид>\n" +
				"        <Наименование>ООО \"Рога и копыта\"</Наименование>\n" +
				"        <ОфициальноеНаименование></ОфициальноеНаименование>\n" +
				"        <ЮридическийАдрес>\n" +
				"          <Представление></Представление>\n" +
				"        </ЮридическийАдрес>\n" +
				"        <Роль>Покупатель</Роль>\n" +
				"      </Контрагент>\n" +
				"    </Контрагенты>\n" +
				"    <Время>12:14:19</Время>\n" +
				"    <Комментарий></Комментарий>\n" +
				"    <Товары>\n" +
				"      <Товар>\n" +
				"        <Ид>33</Ид>\n" +
				"        <ИдКаталога></ИдКаталога>\n" +
				"        <Наименование>Диван</Наименование>\n" +
				"        <БазоваяЕдиница Код=\"796\" НаименованиеПолное=\"Штука\" " +
				"МеждународноеСокращение=\"PCE\">шт\n" +
				"        </БазоваяЕдиница>\n" +
				"        <ЦенаЗаЕдиницу>10000.00</ЦенаЗаЕдиницу>\n" +
				"        <Количество>1.00</Количество>\n" +
				"        <Сумма>10000</Сумма>\n" +
				"        <ЗначенияРеквизитов>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ВидНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ТипНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"        </ЗначенияРеквизитов>\n" +
				"      </Товар>\n" +
				"      <Товар>\n" +
				"        <Ид>34</Ид>\n" +
				"        <ИдКаталога></ИдКаталога>\n" +
				"        <Наименование>Кресло</Наименование>\n" +
				"        <БазоваяЕдиница Код=\"796\" НаименованиеПолное=\"Штука\" " +
				"МеждународноеСокращение=\"PCE\">шт\n" +
				"        </БазоваяЕдиница>\n" +
				"        <Скидки>\n" +
				"          <Скидка>\n" +
				"            <Наименование>Скидка на товар</Наименование>\n" +
				"            <Сумма>900.00</Сумма>\n" +
				"            <УчтеноВСумме>true</УчтеноВСумме>\n" +
				"          </Скидка>\n" +
				"        </Скидки>\n" +
				"        <ЦенаЗаЕдиницу>5100.00</ЦенаЗаЕдиницу>\n" +
				"        <Количество>1.00</Количество>\n" +
				"        <Сумма>5100</Сумма>\n" +
				"        <ЗначенияРеквизитов>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ВидНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ТипНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"        </ЗначенияРеквизитов>\n" +
				"      </Товар>\n" +
				"      <Товар>\n" +
				"        <Ид>348</Ид>\n" +
				"        <ИдКаталога></ИдКаталога>\n" +
				"        <Наименование>12 стульев</Наименование>\n" +
				"        <БазоваяЕдиница Код=\"796\" НаименованиеПолное=\"Штука\" " +
				"МеждународноеСокращение=\"PCE\">шт\n" +
				"        </БазоваяЕдиница>\n" +
				"        <ЦенаЗаЕдиницу>7000.00</ЦенаЗаЕдиницу>\n" +
				"        <Количество>1.00</Количество>\n" +
				"        <Сумма>7000</Сумма>\n" +
				"        <ЗначенияРеквизитов>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ВидНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"          <ЗначениеРеквизита>\n" +
				"            <Наименование>ТипНоменклатуры</Наименование>\n" +
				"            <Значение>Товар</Значение>\n" +
				"          </ЗначениеРеквизита>\n" +
				"        </ЗначенияРеквизитов>\n" +
				"      </Товар>\n" +
				"    </Товары>\n" +
				"  </Документ>\n" +
				"</КоммерческаяИнформация>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
		}

		[Fact]
		public void ProcessingInvalidTagDeclaration()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"	<to>Vasya</to>\n" +
				"	<from>Petya</from>\n" +
				"	<subject>Meeting of graduates</subject>\n" +
				"	<body>Hi! How are you?</body>\n" +
				"</message>"
				;
			const string input2 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<message>\n" +
				"	<to>Vasya</to>\n" +
				"	<from>Petya</from>\n" +
				"	<subject>Meeting of graduates</subject>\n" +
				"	<body>Hi! How are you?</body>"
				;
			const string input3 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<message>\n" +
				"	<to>Vasya</to>\n" +
				"	<from>Petya</from>\n" +
				"	<subject>Meeting of graduates" +
				"	<body>Hi! How are you?</body>\n" +
				"</message>"
				;
			const string input4 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<message>\n" +
				"	<to>Vasya</to>\n" +
				"	Petya</from>\n" +
				"	<subject>Meeting of graduates</subject>\n" +
				"	<body>Hi! How are you?</body>\n" +
				"</message>"
				;

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;
			IList<MinificationErrorInfo> errors3 = minifier.Minify(input3).Errors;
			IList<MinificationErrorInfo> errors4 = minifier.Minify(input4).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(6, errors1[0].LineNumber);
			Assert.Equal(1, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(2, errors2[0].LineNumber);
			Assert.Equal(1, errors2[0].ColumnNumber);

			Assert.Equal(1, errors3.Count);
			Assert.Equal(5, errors3[0].LineNumber);
			Assert.Equal(2, errors3[0].ColumnNumber);

			Assert.Equal(1, errors4.Count);
			Assert.Equal(4, errors4[0].LineNumber);
			Assert.Equal(7, errors4[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidXmlCommentsIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<!-->";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(1, errors1[0].ColumnNumber);
		}
	}
}