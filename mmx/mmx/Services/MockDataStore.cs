using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mmx.Models;

namespace mmx.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;
        List<Item> _GradeItems;
        List<Item> _LessonsItems;
        List<Item> _SentenceItems;
        List<Item> _WordItems;


        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级下册", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }

            #region 模拟年级数据
            _GradeItems = new List<Item>();
            var _GradeMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级下册", Description="This is an item description." },
            };

            foreach (var item in _GradeMockItems)
            {
                _GradeItems.Add(item);
            }
            #endregion

            #region 模拟课文数据
            _LessonsItems = new List<Item>();
            var _LessonsMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "第一课", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第二课", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第三课", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第四课", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第五课", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第六课", Description="This is an item description." },
            };

            foreach (var item in _LessonsMockItems)
            {
                _LessonsItems.Add(item);
            }
            #endregion

            #region 模拟语句数据
            _SentenceItems = new List<Item>();
            var _SentenceMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "第一个语句", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第二个语句", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第三个语句", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第四个语句", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第五个语句", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第六个语句", Description="This is an item description." },
            };

            foreach (var item in _SentenceMockItems)
            {
                _SentenceItems.Add(item);
            }
            #endregion

            #region 模拟单词数据
            _WordItems = new List<Item>();
            var _WordMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "第一个单词", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第二个单词", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第三个单词", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第四个单词", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第五个单词", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "第六个单词", Description="This is an item description." },
            };

            foreach (var item in _WordMockItems)
            {
                _WordItems.Add(item);
            }
            #endregion
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Item>> GetGradeItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_GradeItems);
        }

        public async Task<IEnumerable<Item>> GetLessonsItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_LessonsItems);
        }

        public async Task<IEnumerable<Item>> GetSentenceItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_SentenceItems);
        }

        public async Task<IEnumerable<Item>> GetWordItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_WordItems);
        }
    }
}