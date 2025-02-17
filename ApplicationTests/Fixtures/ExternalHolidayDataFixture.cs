using Core.ViewModels;

namespace ApplicationTests.Fixtures;

public static class ExternalHolidayDataFixture
{
    public static List<ExternalHolidayViewModel> GetSampleData()
        {
            return new List<ExternalHolidayViewModel>
            {
                // 2020 Data
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 1, Month = 1, Year = 2020, DayOfWeek = 3 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Новий Рік" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "New Year's Day" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 6, Month = 1, Year = 2020, DayOfWeek = 1 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Свято" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Holiday" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 7, Month = 1, Year = 2020, DayOfWeek = 2 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Різдво" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Orthodox Christmas" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 8, Month = 3, Year = 2020, DayOfWeek = 7 },
                    // Ignoring observedOn for mapping simplicity
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Міжнародний жіночий день" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "International Women's Day" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 19, Month = 4, Year = 2020, DayOfWeek = 7 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Пасха" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Orthodox Easter Sunday" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 20, Month = 4, Year = 2020, DayOfWeek = 1 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Пасха" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Orthodox Easter Monday" }
                    },
                    HolidayType = "public_holiday"
                },
                // 2021 Data
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 1, Month = 1, Year = 2021, DayOfWeek = 5 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Новий Рік" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "New Year's Day" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 7, Month = 1, Year = 2021, DayOfWeek = 4 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Різдво" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Orthodox Christmas" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 8, Month = 1, Year = 2021, DayOfWeek = 5 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Свято" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Holiday" }
                    },
                    HolidayType = "public_holiday"
                },
                // 2022 Data
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 1, Month = 1, Year = 2022, DayOfWeek = 6 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Новий Рік" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "New Year's Day" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 7, Month = 1, Year = 2022, DayOfWeek = 5 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Різдво" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "Orthodox Christmas" }
                    },
                    HolidayType = "public_holiday"
                },
                new ExternalHolidayViewModel {
                    Date = new ExternalDateViewModel { Day = 8, Month = 3, Year = 2022, DayOfWeek = 2 },
                    Name = new List<ExternalLocalizedTextViewModel> {
                        new ExternalLocalizedTextViewModel { Lang = "uk", Text = "Міжнародний жіночий день" },
                        new ExternalLocalizedTextViewModel { Lang = "en", Text = "International Women's Day" }
                    },
                    HolidayType = "public_holiday"
                }
            };
        }
}